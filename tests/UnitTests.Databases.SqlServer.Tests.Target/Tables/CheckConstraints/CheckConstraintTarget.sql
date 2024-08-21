ALTER TABLE [dbo].[TableTarget]
	ADD CONSTRAINT [CheckConstraintTarget]
	CHECK ([Id] > 0)
