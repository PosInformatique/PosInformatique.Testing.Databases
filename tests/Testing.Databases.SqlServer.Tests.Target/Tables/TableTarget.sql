CREATE TABLE [dbo].[TableTarget]
(
	[Id]			INT			NOT NULL,
	[TargetName]	VARCHAR(50) CONSTRAINT DF_TableTarget_TargetName DEFAULT 'Target',
	[TargetForeignKeyId]	INT NOT NULL,
)
