SELECT
    F.Id AS Id,
    F.Name AS Name,
    F.FilePath AS FilePath,
    F.FileType AS FileType,
    AT.Name AS ActionTypeName,
    AT.IsReadAble AS IsReadAble,
    AT.IsWriteAble AS IsWriteAble,
    AT.IsExecuteAble AS IsExecuteAble,
    DAM.AllowFrom AS AllowFrom,
    DAM.AllowTo AS AllowTo,
    CASE
        WHEN (substr(AllowFrom, 12, 8) = '00:00:00' AND substr(AllowTo, 12, 8) = '00:00:00') THEN 'час не обмежений'
        ELSE (substr(AllowFrom, 12, 5) || ' - ' || substr(AllowTo, 12, 5))
    END AS AvailabilityTimePeriod
FROM DiscretionaryAccessMatrix DAM
LEFT JOIN Files AS F ON F.Id = DAM.FileId
LEFT JOIN ActionTypes AS AT ON AT.Id = DAM.ActionTypeId
WHERE
    DAM.UserId = @UserId 
    AND (
        /*обираємо файл тільки якщо файл типу текст або картина та корситувач має дозволену дію на читання або запис*/
        (F.FileType IN(0, 1) AND (AT.IsReadAble OR AT.IsWriteAble)) 
        /*обираємо файл тільки якщо файл типу exe та корситувач має дозволену дію на виконання*/
        OR (F.FileType = 2 AND AT.IsExecuteAble)
    )
    AND (
        time('now', 'localtime') BETWEEN substr(AllowFrom, 12, 8) AND substr(AllowTo, 12, 8)
        OR (substr(AllowFrom, 12, 8) = '00:00:00' AND substr(AllowTo, 12, 8) = '00:00:00')
    )