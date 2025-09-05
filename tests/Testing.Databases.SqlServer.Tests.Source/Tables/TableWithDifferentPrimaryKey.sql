CREATE TABLE [dbo].[TableWithDifferentPrimaryKey]
(
	[Id] INT NOT NULL,

	CONSTRAINT [PK_TableWithDifferentPrimaryKey_Source] PRIMARY KEY CLUSTERED ([Id] ASC)
)
