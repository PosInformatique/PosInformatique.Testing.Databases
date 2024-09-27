ALTER TABLE [dbo].[TableDifference]
	ADD CONSTRAINT [CheckConstraintDifference]
	CHECK ([Type] > 0)
