CREATE TRIGGER [TriggerDifference]
	ON [dbo].[TableDifference]
	INSTEAD OF INSERT
	AS
	BEGIN
		PRINT 'From source'
	END
