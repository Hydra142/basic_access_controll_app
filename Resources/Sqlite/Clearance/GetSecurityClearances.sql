SELECT
    SC.Id AS Id,
    SC.Lvl AS Lvl,
    SC.Name AS Name,
    SC.ActionTypeId AS ActionTypeId,
    AT.IsReadAble AS IsReadAble,
    AT.IsWriteAble AS IsWriteAble,
    AT.Name AS ActionTypeName
FROM SecurityClearances AS SC
LEFT JOIN ActionTypes AS AT ON AT.Id = SC.ActionTypeId