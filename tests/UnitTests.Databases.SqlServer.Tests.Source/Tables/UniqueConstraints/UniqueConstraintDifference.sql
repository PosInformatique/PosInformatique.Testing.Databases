ALTER TABLE [dbo].[TableDifference]
	ADD CONSTRAINT [UniqueConstraintDifference]
	UNIQUE ([Type], [MaxLength])
