CREATE INDEX [IndexIdentical]
	ON [dbo].[TableIdentical]
	([ForeignKeyId])
    WHERE [ForeignKeyId] > 0
