CREATE INDEX [IndexDifference]
	ON [dbo].[TableDifference]
	([Type], [ForeignKeyId])
    INCLUDE ([Scale], [Precision])
    WHERE [Type] = 'Target'