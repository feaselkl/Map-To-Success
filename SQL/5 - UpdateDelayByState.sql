USE Scratch
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE dbo.UpdateDelayByState
(
@DelayByState dbo.DelayByStateType READONLY
)
AS
BEGIN
	-- I want to ensure that the table gets locked during loading.
	-- That way, two processes cannot try to update the table
	-- at the same time.
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
	BEGIN TRANSACTION

	UPDATE dbs
	SET
		NumberOfFlights = dbs.NumberOfFlights + d.NumberOfFlights,
		NumberOfDelayedFlights = dbs.NumberOfDelayedFlights + d.NumberOfDelayedFlights,
		ArrivalDelay = dbs.ArrivalDelay + d.ArrivalDelay
	FROM @DelayByState d
		INNER JOIN dbo.DelayByState dbs
			ON d.DestinationState = dbs.DestinationState
	
	INSERT INTO dbo.DelayByState 
	(
		DestinationState,
		NumberOfFlights,
		NumberOfDelayedFlights,
		ArrivalDelay
	)
	SELECT
		d.DestinationState,
		d.NumberOfFlights,
		d.NumberOfDelayedFlights,
		d.ArrivalDelay
	FROM @DelayByState d
		LEFT OUTER JOIN dbo.DelayByState dbs
			ON d.DestinationState = dbs.DestinationState
	WHERE
		dbs.DestinationState IS NULL;

	COMMIT TRANSACTION;
END
GO
