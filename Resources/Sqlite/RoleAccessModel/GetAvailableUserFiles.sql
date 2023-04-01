SELECT
    F.Id AS Id,
    F.Name AS Name,
    F.FilePath AS FilePath,
    F.FileType AS FileType,
    AT.Name AS ActionTypeName,
    MAX(AT.IsReadAble) AS IsReadAble,/*дозволене читання*/
    MAX(AT.IsWriteAble) AS IsWriteAble,/*дозволене редагування*/
    MAX(AT.IsExecuteAble) AS IsExecuteAble,/*дозволене виконання*/
    RF.AllowFrom AS AllowFrom,/*час початку доступності*/
    RF.AllowTo AS AllowTo,/*час закінчення доступності*/
    CASE
        WHEN (substr(AllowFrom, 12, 8) = '00:00:00' AND substr(AllowTo, 12, 8) = '00:00:00') THEN 'час не обмежений'
        ELSE (substr(AllowFrom, 12, 5) || ' - ' || substr(AllowTo, 12, 5))
    END AS AvailabilityTimePeriod /*повідомлення яке буде показане користувачу*/
FROM RoleFiles RF
LEFT JOIN ActionTypes AT on AT.Id = RF.ActionTypeId
LEFT JOIN Roles R on R.Id = RF.RoleId
LEFT JOIN Files F on F.Id = RF.FileId
WHERE
    /*Перевіряємо чи роль необхідна для доступу до цього файлу надана користувачу*/
    RF.RoleId IN (SELECT UR.RoleId FROM UserRoles UR WHERE UR.UserId = @UserId)
    /*перевіряємо чи дозволено доступ в даний момент часу (яко обмеження задіяні)*/
    AND (
            time('now', 'localtime') BETWEEN substr(RF.AllowFrom, 12, 8) AND substr(RF.AllowTo, 12, 8)
            OR (substr(RF.AllowFrom, 12, 8) = '00:00:00' AND substr(RF.AllowTo, 12, 8) = '00:00:00')
        )
GROUP BY F.Id
HAVING 
    /*обираємо файл тільки якщо корсистувач має відповідну дозволену дію відповідну до типу файлу*/
    (MAX(F.FileType) IN(0, 1) AND (MAX(AT.IsReadAble) OR MAX(AT.IsWriteAble)))
            OR (MAX(F.FileType) = 2 AND MAX(AT.IsExecuteAble))