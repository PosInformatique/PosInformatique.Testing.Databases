﻿ALTER TABLE [dbo].[TableDifference]
	ADD CONSTRAINT [PrimaryKeyDifference]
	PRIMARY KEY NONCLUSTERED ([Type], [MaxLength])
