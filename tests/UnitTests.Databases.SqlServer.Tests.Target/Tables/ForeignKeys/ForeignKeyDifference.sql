ALTER TABLE [dbo].[TableDifference]
	ADD CONSTRAINT [ForeignKeyDifference]
	FOREIGN KEY ([ForeignKeyId])
	REFERENCES [ReferencedTable] (Id)
	ON UPDATE CASCADE
	ON DELETE CASCADE