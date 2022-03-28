sp_configure 'show advanced options', 1
GO
RECONFIGURE
GO
sp_configure 'clr enabled', 1
GO
RECONFIGURE
GO

USE master
GO

CREATE DATABASE $(DatabaseName)
GO
USE $(DatabaseName)
GO

CREATE SCHEMA Common
GO
CREATE SCHEMA IL
GO
CREATE SCHEMA GL
GO

ALTER DATABASE $(DatabaseName) SET TRUSTWORTHY ON
GO
