CREATE TABLE [dbo].[TableSource]
(
	[Id]			INT			NOT NULL,
	[SourceName]	VARCHAR(50) CONSTRAINT DF_TableSource_SourceName DEFAULT 'Source',
	[SourceForeignKeyId]	INT NOT NULL,
)
