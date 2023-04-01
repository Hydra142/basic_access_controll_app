/*Достаємо матрицю роль - файл*/
SELECT
	M.Id AS Id,
	R.Id AS RoleId,
	F.Id AS FileId,
	F.Name AS FileName,
	IFNULL(AT.Id, 1) AS ActionTypeId,
	M.AllowFrom AS AllowFrom,
	M.AllowTo AS AllowTo,
	M.Id AS IsActive /*перевірка чи активне дане правило*/
FROM Files AS F
LEFT JOIN Roles AS R ON R.Id = @RoleId
LEFT JOIN RoleFiles AS M ON M.RoleId = R.Id AND M.FileId = F.Id
LEFT JOIN ActionTypes AS AT ON AT.Id = M.ActionTypeId
WHERE R.Id = @RoleId
ORDER BY F.Id