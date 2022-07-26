module MapToSuccess.DatabaseAccess

open FSharp.Data

// NOTE:  change this string to match your server.
// This assumes that you are running from a Docker container.
// Linux versions of Docker don't like host.docker.internal as the name
// so I am using the default bridge IP address.
[<Literal>]
let connectionString =
    @"Data Source=172.17.0.1,51433;Initial Catalog=MapToSuccess;uid=sa;pwd=SomeBadP@ssword3"

// Simple SQL query
type AirportSql = 
    SqlCommandProvider<"SELECT IATA, Airport, City, State, Country, Lat, Long FROM dbo.Airports WHERE IATA = @IATA", connectionString>

let getAirportSql iata =
    let conn = new System.Data.SqlClient.SqlConnection(connectionString)
    conn.Open()
    let airport = AirportSql.Create(conn).Execute(iata) |> Seq.exactlyOne
    airport

// Stored procedure call
type AirportByIata = SqlCommandProvider<"EXEC dbo.GetAirportByIATA @IATA", connectionString>

let getAirportSp iata =
    let airportSP = new AirportByIata(connectionString)
    airportSP.Execute(iata) |> Seq.exactlyOne

// Stored procedure with TVP
type UpdateDelayByState = SqlCommandProvider<"EXEC dbo.UpdateDelayByState @DelayByState", connectionString>
type DelayByStateTVP = UpdateDelayByState.DelayByStateType

let writeDelayByState stateDelays =
    let delaySP = new UpdateDelayByState(connectionString)
    delaySP.Execute(stateDelays) |> ignore
