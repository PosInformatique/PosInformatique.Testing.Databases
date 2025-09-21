CREATE TABLE [dbo].[TableWithDifferentPrimaryKey]
(
	[Id] INT IDENTITY (10, 20) NOT NULL,

	CONSTRAINT [PK_TableWithDifferentPrimaryKey_Source] PRIMARY KEY CLUSTERED ([Id] ASC)
)
