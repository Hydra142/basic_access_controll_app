SELECT
    F.Id AS Id,
    F.Name AS Name,
    F.FilePath AS FilePath,
    SC.Name AS ClearanceName,
    AT.Name AS ActionTypeName,
    AT.IsReadAble AS IsReadAble,
    AT.IsWriteAble AS IsWriteAble
FROM Files F
CROSS JOIN Users AS U
LEFT JOIN SecurityClearances AS SC ON SC.Id = F.MinimumClearanceId
LEFT JOIN ActionTypes AS AT ON AT.Id = U.ActionTypeId
WHERE U.Id = @UserId AND  SC.Lvl <= (
                    SELECT
                        MAX(USC.Lvl) AS Lvl
                    FROM Users AS U
                    LEFT JOIN SecurityClearances AS USC ON USC.Id = U.ClearanceId
                    WHERE U.Id = @UserId
                    GROUP BY U.Id
                );
