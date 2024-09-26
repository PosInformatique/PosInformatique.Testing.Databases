CREATE TABLE [dbo].[TableDifference]
(
	[Type]		VARCHAR(50) NOT NULL,
	[Nullable]	VARCHAR(50) NOT NULL,
	[Precision]	DECIMAL(5, 2) NOT NULL,	-- Reverse Precision/MaxLength to check position.
	[MaxLength]	VARCHAR(20) NOT NULL,
	[Scale]		DECIMAL(10, 4) NOT NULL,
	[Identity]	INT NOT NULL,
	[ForeignKeyId] INT NULL,
    [Computed]  AS [Scale] - [Precision],
    [TargetColumn] INT NOT NULL,
    [IdenticalColumn] INT NOT NULL,
)
