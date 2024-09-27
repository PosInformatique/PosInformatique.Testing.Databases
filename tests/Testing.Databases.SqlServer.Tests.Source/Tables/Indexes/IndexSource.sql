CREATE INDEX [IndexSource]
	ON [dbo].[TableSource]
	([SourceName])
    INCLUDE ([SourceForeignKeyId])
    WHERE [SourceName] = ''
