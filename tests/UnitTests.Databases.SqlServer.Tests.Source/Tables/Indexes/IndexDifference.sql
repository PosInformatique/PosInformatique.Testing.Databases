CREATE INDEX [IndexDifference]
	ON [dbo].[TableDifference]
	([ForeignKeyId], [Type])
    INCLUDE ([Precision], [Scale], [Identity])
    WHERE [Type] = 1234