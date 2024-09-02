ALTER TABLE [dbo].[TableDifference]
	ADD CONSTRAINT [ForeignKeyDifference]
	FOREIGN KEY ([ForeignKeyId])
	REFERENCES [ReferencedTable] (Id)
	ON UPDATE SET NULL
	ON DELETE NO ACTION
