UPDATE [Users] SET
[UserName] = @UserName,
[Password] = @Password,
[IsAdmin] = @IsAdmin,
[PasswordTypeId] = @PasswordTypeId,
[ClearanceId] = @ClearanceId,
[ActionTypeId] = @ActionTypeId,
[AccessControlModelId] = @AccessControlModelId,
[PasswordActiveDays] = @PasswordActiveDays
WHERE [Id] = @Id;
