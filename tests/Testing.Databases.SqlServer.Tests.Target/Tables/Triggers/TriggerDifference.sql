CREATE TRIGGER [TriggerDifference]
	ON [dbo].[TableDifference]
	FOR INSERT
	AS
	BEGIN
		PRINT 'From target'
	END
