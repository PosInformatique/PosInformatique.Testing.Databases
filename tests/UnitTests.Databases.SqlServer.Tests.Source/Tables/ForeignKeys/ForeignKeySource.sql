ALTER TABLE [dbo].[TableSource]
	ADD CONSTRAINT [ForeignKeySource]
	FOREIGN KEY ([SourceForeignKeyId])
	REFERENCES [ReferencedTable] (Id)
