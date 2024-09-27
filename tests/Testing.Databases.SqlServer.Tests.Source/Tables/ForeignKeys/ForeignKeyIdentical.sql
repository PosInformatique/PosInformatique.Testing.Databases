ALTER TABLE [dbo].[TableIdentical]
	ADD CONSTRAINT [ForeignKeyIdentical]
	FOREIGN KEY ([ForeignKeyId])
	REFERENCES [ReferencedTable] (Id)
