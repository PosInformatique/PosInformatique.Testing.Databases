CREATE INDEX [IndexSource]
	ON [dbo].[TableSource]
	([SourceName])
    WHERE [SourceName] = ''
