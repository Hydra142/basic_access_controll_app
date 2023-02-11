DROP TABLE IF EXISTS [PaswordTypes];
CREATE TABLE IF NOT EXISTS [PaswordTypes] (
  [Id] INTEGER PRIMARY KEY AUTOINCREMENT
, [Name] NVARCHAR(64) NOT NULL
, [ValidationRegex] TEXT NOT NULL
, [Description] TEXT NOT NULL
, [Created] DATETIME default current_timestamp
);
INSERT OR IGNORE INTO PaswordTypes ([Name], [ValidationRegex], [Description]) VALUES
('Simple', '.', 'Пароль повивнен містити мінімум один символ'),
('Complex', '^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$', 'Пароль має містити мінімум 8 символів, містити сиволи різного регістру та цифри');

DROP TABLE IF EXISTS [Users];
CREATE TABLE IF NOT EXISTS [Users] (
  [Id] INTEGER PRIMARY KEY AUTOINCREMENT
, [UserName] TEXT NOT NULL
, [Password] TEXT NULL
, [IsAdmin] BOOLEAN NOT NULL default 0
, [PasswordTypeId] INTEGER NOT NULL
, [Created] DATETIME default current_timestamp
, FOREIGN KEY ([PasswordTypeId]) REFERENCES PaswordTypes(Id)
);
INSERT INTO Users ([UserName], [Password], [PasswordTypeId], [IsAdmin]) VALUES
('Redchych', '', 1, 1), ('Ivanov_1', '', 2, 0)