CREATE TABLE [dbo].[TableWithDifferentPrimaryKey]
(
	[Id] INT NOT NULL,

	CONSTRAINT [PK_TableWithDifferentPrimaryKey_Target] PRIMARY KEY CLUSTERED ([Id] ASC)
)
