﻿CREATE TRIGGER [TriggerTarget]
	ON [dbo].[TableTarget]
	FOR DELETE, INSERT, UPDATE
	AS
	BEGIN
		SET NOCOUNT ON
	END