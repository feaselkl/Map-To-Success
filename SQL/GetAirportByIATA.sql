USE [Scratch]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GetAirportByIATA]
@IATA VARCHAR(4)
AS
BEGIN
	SELECT
		a.IATA,
		a.Airport,
		a.City,
		a.State,
		a.Country,
		a.Lat,
		a.Long
	FROM dbo.Airports a
	WHERE
		a.IATA = @IATA;
END
GO
