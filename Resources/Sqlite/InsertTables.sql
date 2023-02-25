﻿DROP TABLE IF EXISTS [PaswordTypes];
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
  Name TEXT NOT NULL UNIQUE
);
INSERT INTO ActionTypes ([Id],[Name],[IsReadAble],[IsWriteAble]) VALUES (1, 'r', 1, 0), (2, 'r, w', 1, 1);


DROP TABLE IF EXISTS [SecurityClearances];
CREATE TABLE IF NOT EXISTS [SecurityClearances] (
  Id INTEGER PRIMARY KEY AUTOINCREMENT,
  Lvl INTEGER NOT NULL default 1,
  Name TEXT NOT NULL UNIQUE,
  ActionTypeId INTEGER NOT NULL,
  FOREIGN KEY ([ActionTypeId]) REFERENCES ActionTypes(Id)
);
INSERT INTO SecurityClearances ([Id],[Name],[Lvl],[ActionTypeId]) VALUES
(1, 'Не таємно (r)', 1, 1),
(2, 'Не таємно (r,w)', 2, 2),
(3, 'Службове (r)', 3, 1),
(4, 'Службове (r,w)', 4, 2),
(5, 'Таємно (r)', 5, 1),
(6, 'Таємно (r,w)', 6, 2);

DROP TABLE IF EXISTS [Users];
CREATE TABLE IF NOT EXISTS [Users] (
  [Id] INTEGER PRIMARY KEY AUTOINCREMENT
, [UserName] TEXT NOT NULL
, [Password] TEXT NULL
, [IsAdmin] BOOLEAN NOT NULL default 0
, [PasswordTypeId] INTEGER NOT NULL
, [Created] DATETIME default current_timestamp
, ClearanceId INTEGER NOT NULL default 1
, FOREIGN KEY ([PasswordTypeId]) REFERENCES PaswordTypes(Id)
, FOREIGN KEY (ClearanceId) REFERENCES SecurityClearances (Id)
);
INSERT INTO Users ([Id],[UserName], [Password], [PasswordTypeId], [IsAdmin]) VALUES
(1, 'Admin', 'q', 1, 1),
(2, 'Редчич ', 'q', 1, 0);


DROP TABLE IF EXISTS [Files];
CREATE TABLE Files (
  Id INTEGER PRIMARY KEY AUTOINCREMENT,
  Name TEXT NOT NULL UNIQUE,
  FilePath TEXT NOT NULL,
  MinimumClearanceId INTEGER NOT NULL,
  FOREIGN KEY (MinimumClearanceId) REFERENCES SecurityClearances (Id)
);
INSERT INTO Files ([Id],[Name],[FilePath],[MinimumClearanceId]) VALUES
(1, 'ЗвичайнийФайл','D:\LabsData\TBD\TBD_Redchych\Data\TextFile1.txt', 1),
(2, 'СлужбовийФайл','D:\LabsData\TBD\TBD_Redchych\Data\TextFile2.txt', 4),
(3, 'ТаємнийФайл','D:\LabsData\TBD\TBD_Redchych\Data\TextFile3.txt', 6);


