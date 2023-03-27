SELECT
	M.Id AS Id,
	U.Id AS UserId,
	F.Id AS FileId,
	F.Name AS FileName,
	IFNULL(AT.Id, 1) AS ActionTypeId,
	M.AllowFrom AS AllowFrom,
	M.AllowTo AS AllowTo,
	M.Id AS IsActive /*перевірка чи активне дане правило*/
FROM Files AS F
LEFT JOIN Users AS U ON U.Id = @UserId
LEFT JOIN DiscretionaryAccessMatrix AS M ON M.UserId = U.Id AND M.FileId = F.Id
LEFT JOIN ActionTypes AS AT ON AT.Id = M.ActionTypeId
WHERE U.Id = @UserId
ORDER BY F.Name