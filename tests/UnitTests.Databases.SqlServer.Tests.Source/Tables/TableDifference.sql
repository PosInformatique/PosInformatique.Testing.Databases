CREATE TABLE [dbo].[TableDifference]
(
	[Type]		INT			NOT NULL,
	[Nullable]	VARCHAR(50) NULL,
	[MaxLength]	VARCHAR(50) NOT NULL,
	[Precision]	DECIMAL(10, 2) NOT NULL,
	[Scale]		DECIMAL(10, 2) NOT NULL,
	[Identity]	INT NOT NULL IDENTITY,
	[ForeignKeyId] INT NULL,
    [Computed]  AS [Scale] + [Precision],
    [SourceColumn] INT NOT NULL,
)
