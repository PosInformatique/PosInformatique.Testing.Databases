ALTER TABLE [dbo].[TableTarget]
	ADD CONSTRAINT [ForeignKeyTarget]
	FOREIGN KEY ([TargetForeignKeyId])
	REFERENCES [ReferencedTable] (Id)
