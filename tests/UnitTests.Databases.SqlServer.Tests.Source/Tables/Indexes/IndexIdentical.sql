CREATE INDEX [IndexIdentical]
	ON [dbo].[TableIdentical]
	([ForeignKeyId])
    INCLUDE ([IncludeColumn])
    WHERE [ForeignKeyId] > 0
