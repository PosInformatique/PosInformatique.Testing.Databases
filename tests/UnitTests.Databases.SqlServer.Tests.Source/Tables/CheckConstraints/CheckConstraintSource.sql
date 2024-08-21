ALTER TABLE [dbo].[TableSource]
	ADD CONSTRAINT [CheckConstraintSource]
	CHECK ([Id] > 0)
