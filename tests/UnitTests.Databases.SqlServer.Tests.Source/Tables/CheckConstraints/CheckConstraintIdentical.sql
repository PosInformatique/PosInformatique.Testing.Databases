ALTER TABLE [dbo].[TableIdentical]
	ADD CONSTRAINT [CheckConstraintIdentical]
	CHECK ([Id] > 0)
