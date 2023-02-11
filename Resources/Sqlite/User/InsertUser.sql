INSERT INTO Users ([UserName], [Password], [PasswordTypeId], [IsAdmin]) VALUES
(@Name, '', @PasswordTypeId, @IsAdmin);
SELECT
    Users.Id AS [Id],
    Users.UserName AS [Name],
    Users.Password AS [Password],
    Users.IsAdmin AS [IsAdmin],
    PaswordTypes.Id AS [PasswordTypeId],
    PaswordTypes.Name AS [PasswordTypeName],
    PaswordTypes.ValidationRegex AS [PasswordValidationRegex],
    PaswordTypes.Description AS [PasswordTypeDescription]
FROM Users
LEFT JOIN PaswordTypes ON PaswordTypes.Id = PasswordTypeId
WHERE Users.Id = last_insert_rowid();