UPDATE [Users] SET
[Password] = @Password,
[IsAdmin] = @IsAdmin,
[PasswordTypeId] = @PasswordTypeId
WHERE [Id] = @Id;
