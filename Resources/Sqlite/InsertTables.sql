DROP TABLE IF EXISTS [PaswordTypes];
CREATE TABLE IF NOT EXISTS [PaswordTypes] (
  [Id] INTEGER PRIMARY KEY AUTOINCREMENT
, [Name] NVARCHAR(64) NOT NULL
, [ValidationRegex] TEXT NOT NULL
, [Description] TEXT NOT NULL
, [Created] DATETIME default current_timestamp
);
INSERT OR IGNORE INTO PaswordTypes ([Id],[Name], [ValidationRegex], [Description]) VALUES
(1, 'Простий', '.', 'Пароль повивнен містити мінімум один символ'),
(2, 'Складний', '^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$', 'Пароль має містити мінімум 8 символів, містити сиволи різного регістру та цифри');

DROP TABLE IF EXISTS [ActionTypes];
CREATE TABLE IF NOT EXISTS [ActionTypes] (
  Id INTEGER PRIMARY KEY AUTOINCREMENT,
  IsReadAble BOOLEAN NOT NULL default 0,
  IsWriteAble BOOLEAN NOT NULL default 0,
  IsExecuteAble BOOLEAN NOT NULL default 0,
  Name TEXT NOT NULL UNIQUE
);
INSERT INTO ActionTypes ([Id],[Name],[IsReadAble],[IsWriteAble], [IsExecuteAble]) VALUES
(1, '(r)', 1, 0, 0),
(2, '(r, w)', 1, 1, 0),
(3, '(r, w, e)', 1, 1, 1),
(4, '(r, e)', 1, 0, 1),
(5, '(e)', 0, 0, 1);


DROP TABLE IF EXISTS [SecurityClearances];
CREATE TABLE IF NOT EXISTS [SecurityClearances] (
  Id INTEGER PRIMARY KEY AUTOINCREMENT,
  Lvl INTEGER NOT NULL default 1,
  Name TEXT NOT NULL UNIQUE
);
INSERT INTO SecurityClearances ([Id],[Name],[Lvl]) VALUES
(1, 'Не таємно', 1),
(2, 'Службове', 4),
(3, 'Таємно', 6);

DROP TABLE IF EXISTS [Users];
CREATE TABLE IF NOT EXISTS [Users] (
  [Id] INTEGER PRIMARY KEY AUTOINCREMENT
, [UserName] TEXT NOT NULL
, [Password] TEXT NULL
, [IsAdmin] BOOLEAN NOT NULL default 0
, [PasswordTypeId] INTEGER NOT NULL
, [Created] DATETIME default current_timestamp
, [PasssworExpirationDate] DATETIME default NULL
, [PasswordActiveDays] INTEGER NOT NULL default 30
, ClearanceId INTEGER NOT NULL default 1
, ActionTypeId INTEGER NOT NULL default 1
, AccessControlModelId INTEGER NOT NULL default 0
, FOREIGN KEY ([ActionTypeId]) REFERENCES ActionTypes(Id)
, FOREIGN KEY ([PasswordTypeId]) REFERENCES PaswordTypes(Id)
, FOREIGN KEY (ClearanceId) REFERENCES SecurityClearances (Id)
);
INSERT INTO Users ([Id],[UserName], [Password], [PasswordTypeId], [IsAdmin], [ActionTypeId]) VALUES
(1, 'Admin', '', 1, 1, 2),
(2, 'Редчич 1', '', 1, 0, 2),
(3, 'Редчич 2', '', 1, 0, 2),
(4, 'Редчич 3', '', 1, 0, 2),
(5, 'Редчич 4', '', 1, 0, 2),
(6, 'Редчич 5', '', 1, 0, 2);

DROP TABLE IF EXISTS [PasswordHistory];
CREATE TABLE PasswordHistory (
  Id INTEGER PRIMARY KEY AUTOINCREMENT,
  UserId INTEGER NOT NULL,
  Password INTEGER NOT NULL,
  FOREIGN KEY (UserId) REFERENCES Users (Id)
);

/*РОЛЬОВА СТАРТ*/
DROP TABLE IF EXISTS [Roles];
CREATE TABLE Roles (
  Id INTEGER PRIMARY KEY AUTOINCREMENT,
  Name TEXT NOT NULL UNIQUE
);

DROP TABLE IF EXISTS [UserRoles];
CREATE TABLE UserRoles (
  Id INTEGER PRIMARY KEY AUTOINCREMENT,
  UserId INTEGER NOT NULL,
  RoleId INTEGER NOT NULL,
  FOREIGN KEY (RoleId) REFERENCES Roles (Id),
  FOREIGN KEY (UserId) REFERENCES Users (Id)
);
CREATE UNIQUE INDEX [UserRoles_UI] on [UserRoles] (UserId, RoleId);

DROP TABLE IF EXISTS [RoleFiles];
CREATE TABLE RoleFiles (
  Id INTEGER PRIMARY KEY AUTOINCREMENT,
  RoleId INTEGER NOT NULL,
  FileId INTEGER NOT NULL,
  ActionTypeId INTEGER NOT NULL,
  AllowFrom DATETIME default NULL,
  AllowTo DATETIME default NULL,
  FOREIGN KEY (RoleId) REFERENCES Roles (Id),
  FOREIGN KEY (FileId) REFERENCES Files (Id),
  FOREIGN KEY (ActionTypeId) REFERENCES ActionTypes (Id)
);
CREATE UNIQUE INDEX [RoleFiles_UI] on [RoleFiles] (RoleId, FileId);
/*РОЛЬОВА КІНЕЦЬ*/
DROP TABLE IF EXISTS [Files];
CREATE TABLE Files (
  Id INTEGER PRIMARY KEY AUTOINCREMENT,
  Name TEXT NOT NULL UNIQUE,
  FilePath TEXT NOT NULL,
  FileType INTEGER NOT NULL default 0,
  MinimumClearanceId INTEGER NOT NULL,
  FOREIGN KEY (MinimumClearanceId) REFERENCES SecurityClearances (Id)
);

DROP TABLE IF EXISTS [DiscretionaryAccessMatrix];
CREATE TABLE DiscretionaryAccessMatrix (
  Id INTEGER PRIMARY KEY AUTOINCREMENT,
  UserId INTEGER NOT NULL,
  FileId INTEGER NOT NULL,
  ActionTypeId INTEGER NOT NULL,
  AllowFrom DATETIME default NULL,
  AllowTo DATETIME default NULL,
  FOREIGN KEY (UserId) REFERENCES Users (Id),
  FOREIGN KEY (FileId) REFERENCES Files (Id),
  FOREIGN KEY (ActionTypeId) REFERENCES ActionTypes (Id)
);
CREATE UNIQUE INDEX [DiscretionaryAccessMatrix_UI] on [DiscretionaryAccessMatrix] (UserId, FileId);

insert into main.Files (Id, Name, FilePath, FileType, MinimumClearanceId)
values  (1, 'Txt 1', 'D:\LabsData\TBD\TBD_Redchych\Data\TextFile1.txt', 0, 1),
        (2, 'Txt 2', 'D:\LabsData\TBD\TBD_Redchych\Data\TextFile2.txt', 0, 1),
        (3, 'Txt 3', 'D:\LabsData\TBD\TBD_Redchych\Data\TextFile3.txt', 0, 1),
        (4, 'Exe 1', 'D:\LabsData\TBD\TBD_Redchych\Data\secret_executable_file.exe', 2, 1),
        (6, 'Img 1', 'D:\LabsData\TBD\TBD_Redchych\Data\ImgFile.png', 1, 1);

CREATE TRIGGER insert_password_history
AFTER UPDATE ON Users
FOR EACH ROW
/*тільки якщо пароль змінився*/
WHEN NEW.password <> OLD.password
BEGIN
  /*створюємо новий запис історії*/
  INSERT INTO PasswordHistory (UserId, password)
  VALUES (NEW.id, NEW.password);
END;

CREATE TRIGGER update_password_expiration
BEFORE UPDATE ON Users
FOR EACH ROW
/*тільки якщо пароль змінився*/
WHEN NEW.password <> OLD.password
BEGIN
  UPDATE Users
  /*обчислюємо нову дату закінчення дії*/
  SET PasssworExpirationDate = datetime('now', '+' || NEW.PasswordActiveDays || ' days')
  WHERE Id = NEW.id;
END;
