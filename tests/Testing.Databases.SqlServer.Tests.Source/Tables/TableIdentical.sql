CREATE TABLE [dbo].[TableIdentical]
(
	[Id] INT IDENTITY (4, 5) NOT NULL,
	[ForeignKeyId] INT NOT NULL CONSTRAINT DF_TableIdentical_ForeignKeyId DEFAULT (1 + 2 + 3),
	[IncludeColumn] INT NOT NULL,
)
