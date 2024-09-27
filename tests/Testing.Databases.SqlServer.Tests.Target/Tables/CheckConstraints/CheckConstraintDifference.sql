ALTER TABLE [dbo].[TableDifference]
	ADD CONSTRAINT [CheckConstraintDifference]
	CHECK ([Type] IN (1, 2))
