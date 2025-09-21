ALTER TABLE [dbo].[TableDifference]
	ADD CONSTRAINT [UniqueConstraintDifferenceMissingInTarget]
	UNIQUE NONCLUSTERED ([Scale])
