CREATE TABLE [dbo].[TableWithDifferentPrimaryKey]
(
	[Id] INT IDENTITY (1, 2) NOT NULL,

	CONSTRAINT [PK_TableWithDifferentPrimaryKey_Target] PRIMARY KEY CLUSTERED ([Id] ASC)
)
