CREATE INDEX [IndexDifference]
	ON [dbo].[TableDifference]
	([Type], [ForeignKeyId])
    WHERE [Type] = 'Target'