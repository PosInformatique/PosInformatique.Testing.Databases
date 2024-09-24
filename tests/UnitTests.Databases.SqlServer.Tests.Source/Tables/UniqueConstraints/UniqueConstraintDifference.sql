ALTER TABLE [dbo].[TableDifference]
	ADD CONSTRAINT [UniqueConstraintDifference]
	UNIQUE CLUSTERED ([Type], [MaxLength])
