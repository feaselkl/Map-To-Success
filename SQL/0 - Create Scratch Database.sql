USE [master]
GO
IF (DB_ID('Scratch') IS NULL)
BEGIN
	CREATE DATABASE Scratch;
END
GO
