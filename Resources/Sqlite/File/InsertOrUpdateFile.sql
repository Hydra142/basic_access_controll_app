INSERT OR REPLACE INTO Files (Id, Name, FilePath, FileType, MinimumClearanceId)
VALUES (@Id, @Name, @FilePath, @FileType, @MinimumClearanceId);
SELECT
	Id,
	Name,
	FilePath,
	FileType,
	MinimumClearanceId
FROM Files WHERE Id = last_insert_rowid();
/*ON DUPLICATE KEY UPDATE Name = @Name, FilePath = @FilePath, FileType = @FileType, MinimumClearanceId =@MinimumClearanceId*/