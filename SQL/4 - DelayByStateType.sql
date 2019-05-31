USE [Scratch]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS
(
	SELECT 1
	FROM sys.table_types t
	WHERE
		T.name = N'DelayByStateType'
)
BEGIN
	CREATE TYPE dbo.DelayByState AS TABLE
	(
		DestinationState VARCHAR(2) NOT NULL,
		NumberOfFlights INT NOT NULL,
		NumberOfdelayedFlights INT NOT NULL,
		ArrivalDelay INT NOT NULL
	);
END
GO
