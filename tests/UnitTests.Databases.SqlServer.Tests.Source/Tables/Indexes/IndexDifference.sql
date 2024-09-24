CREATE INDEX [IndexDifference]
	ON [dbo].[TableDifference]
	([ForeignKeyId], [Type])
    WHERE [Type] = 1234