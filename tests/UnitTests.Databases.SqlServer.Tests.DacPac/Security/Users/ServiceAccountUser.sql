CREATE USER [ServiceAccountUser]
	FOR LOGIN [ServiceAccountLogin]
	WITH DEFAULT_SCHEMA = dbo

GO

GRANT CONNECT TO [ServiceAccountUser]

GO

GRANT SELECT, INSERT ON [MyTable] TO [ServiceAccountUser]