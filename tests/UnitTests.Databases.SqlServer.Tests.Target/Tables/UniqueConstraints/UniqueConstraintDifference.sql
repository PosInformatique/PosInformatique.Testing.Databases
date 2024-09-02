ALTER TABLE [dbo].[TableDifference]
	ADD CONSTRAINT [UniqueConstraintDifference]
	UNIQUE ([MaxLength], [Type])
