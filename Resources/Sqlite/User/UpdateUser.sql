UPDATE [Users] SET
[UserName] = @UserName,
[Password] = @Password,
[IsAdmin] = @IsAdmin,
[PasswordTypeId] = @PasswordTypeId,
[ClearanceId] = @ClearanceId
WHERE [Id] = @Id;
