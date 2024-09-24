CREATE INDEX [IndexTarget]
	ON [dbo].[TableTarget]
	([TargetName])
    WHERE [TargetName] = ''
