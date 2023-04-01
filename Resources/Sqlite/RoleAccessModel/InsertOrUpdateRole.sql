INSERT OR REPLACE INTO Roles (Id, Name)
VALUES (@Id, @Name);
SELECT
	*
FROM Roles WHERE Id = last_insert_rowid();
