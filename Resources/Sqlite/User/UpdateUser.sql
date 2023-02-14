UPDATE [Users] SET
[UserName] = @UserName,
[Password] = @Password,
[IsAdmin] = @IsAdmin,
[PasswordTypeId] = @PasswordTypeId
WHERE [Id] = @Id;
