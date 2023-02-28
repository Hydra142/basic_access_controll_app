SELECT
    Users.Id AS [Id],
    Users.UserName AS [Name],
    Users.Password AS [Password],
    Users.IsAdmin AS [IsAdmin],
    Users.ClearanceId AS [ClearanceId],
    Users.ActionTypeId AS [ActionTypeId],
    PaswordTypes.Id AS [PasswordTypeId],
    PaswordTypes.Name AS [PasswordTypeName],
    PaswordTypes.ValidationRegex AS [PasswordValidationRegex],
    PaswordTypes.Description AS [PasswordTypeDescription]
FROM Users
LEFT JOIN PaswordTypes ON PaswordTypes.Id = PasswordTypeId