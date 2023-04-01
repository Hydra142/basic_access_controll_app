/*Достаємо матрицю користувач - роль*/
SELECT
	UR.Id AS Id,
	R.Id AS RoleId,
	R.Name AS RoleName,
	U.Id AS UserId,
	UR.Id AS IsActive /*перевірка чи активне дане правило*/
FROM Roles AS R
LEFT JOIN Users AS U ON U.Id = @UserId
LEFT JOIN UserRoles AS UR ON UR.RoleId = R.Id AND UR.UserId = U.Id
WHERE U.Id = @UserId
ORDER BY R.Id