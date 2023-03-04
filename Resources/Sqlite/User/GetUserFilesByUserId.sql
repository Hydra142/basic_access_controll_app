SELECT
    F.Id AS Id,
    F.Name AS Name, /*Назва файлу*/
    F.FilePath AS FilePath,
    F.FileType AS FileType,
    SC.Name AS ClearanceName,/*Назва рівня доступу*/
    AT.Name AS ActionTypeName,
    AT.IsReadAble AS IsReadAble, /*дозволені дії*/
    AT.IsWriteAble AS IsWriteAble,
    AT.IsExecuteAble AS IsExecuteAble
/*Вибір всіх файлів з таблиці*/
FROM Files F
/*Створюємо всі можливі комбідації файл - користувач*/
CROSS JOIN Users AS U
/*приєднуємо відповідний рівень доступу цього файлу*/
LEFT JOIN SecurityClearances AS SC ON SC.Id = F.MinimumClearanceId
/*приєднуємо дозволену дію*/
LEFT JOIN ActionTypes AS AT ON AT.Id = U.ActionTypeId
WHERE
/*обираємо файли які доступні відповідному корситувачу*/
U.Id = @UserId

AND (
    /*обираємо файл тільки якщо файл типу текст або картина та корситувач має дозволену дію на читання або запис*/
    (F.FileType IN(0, 1) AND (AT.IsReadAble OR AT.IsWriteAble)) 
    /*обираємо файл тільки якщо файл типу exe та корситувач має дозволену дію на виконання*/
    OR (F.FileType = 2 AND AT.IsExecuteAble)
)
/*перевіряємо чи мінімальний рівень доступу файлу менший або рівний рівню фоступу корситувача*/
AND  SC.Lvl <= (
                    SELECT
                        /*обираємо максимальний рівень доступу доступний корситувачу*/
                        MAX(USC.Lvl) AS Lvl
                    FROM Users AS U
                    LEFT JOIN SecurityClearances AS USC ON USC.Id = U.ClearanceId
                    WHERE U.Id = @UserId
                    GROUP BY U.Id
                );
