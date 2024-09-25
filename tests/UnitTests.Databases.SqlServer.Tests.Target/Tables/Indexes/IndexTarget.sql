CREATE INDEX [IndexTarget]
	ON [dbo].[TableTarget]
	([TargetName])
    INCLUDE ([TargetForeignKeyId])
    WHERE [TargetName] = ''
