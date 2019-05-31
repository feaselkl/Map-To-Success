USE [Scratch]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF (OBJECT_ID('dbo.DelayByState') IS NULL)
BEGIN
	CREATE TABLE dbo.DelayByState
	(
		DestinationState VARCHAR(2) NOT NULL,
		NumberOfFlights INT NOT NULL,
		NumberOfdelayedFlights INT NOT NULL,
		ArrivalDelay INT NOT NULL,
		CONSTRAINT [PK_DelayByState] PRIMARY KEY CLUSTERED(DestinationState)
	);
END
GO
