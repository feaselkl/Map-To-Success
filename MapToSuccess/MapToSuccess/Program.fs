module MapToSuccess.Console

open System

// Function call for option 1.
let findMovieSales title =
    let movies = MapToSuccess.Movies.buildSampleMovies
    let movieSales = movies |> MapToSuccess.Movies.buildSampleMovieSales
    let riddick = movies |> Seq.filter (fun m -> m.Title = title) |> Seq.head
    let riddickSales = MapToSuccess.Movies.sumMovieSales riddick movieSales
    let rating = match riddick.Rating with
                        | Some r -> r.ToString()
                        | None -> "Unknown"
    printfn "Our theater made %i in ticket revenue by showing the film %s, a film rated %s" riddickSales riddick.Title rating |> ignore

// Function call for option 2.
let printWorldBankData countryCode =
    let gd = MapToSuccess.JsonTypeProvider.getGovernmentDebt countryCode
                |> Seq.sortBy (fun g -> g.Year)
    for g in gd do
        let debtOutput = match g.PercentDebt with
                                | Some pd -> pd.ToString("#,#.00")
                                | None -> "UNKNOWN"
        printfn "Percent of government debt to GDP for %s in %i was %s" g.CountryName g.Year debtOutput |> ignore

// Function call for option 3.
let printMissingIndex queryPlanLocation =
    let ix = MapToSuccess.XmlTypeProvider.returnMissingIndex queryPlanLocation
    printfn "Maybe think about an index on %s.%s.%s on (%s) INCLUDE (%s)" ix.DatabaseName ix.SchemaName ix.TableName ix.Columns ix.IncludedColumns |> ignore

// Function call for option 4.
let printAirportDetails airportName =
    let airport = MapToSuccess.DatabaseAccess.getAirportSql airportName
    printfn "%s is in %s, %s and is at (%f, %f)" airport.IATA.Value airport.City.Value airport.State.Value airport.Lat.Value airport.Long.Value |> ignore

// Function call for option 5.
let printAirportDetailsSP airportName =
    let airport = MapToSuccess.DatabaseAccess.getAirportSp airportName
    printfn "%s is in %s, %s and is at (%f, %f)" airport.IATA.Value airport.City.Value airport.State.Value airport.Lat.Value airport.Long.Value |> ignore

// Function call for option 6.
// This isn't a commmon pattern: typically, functions have arguments in functional programming languages.
let writeToSqlWithTvp () =
    let oh = MapToSuccess.DatabaseAccess.DelayByStateTVP("OH", 12345, 123, 100)
    let nc = MapToSuccess.DatabaseAccess.DelayByStateTVP("NC", 23456, 234, 200)
    let records = [oh; nc]
    MapToSuccess.DatabaseAccess.writeDelayByState records |> ignore

// Function calls for option 7.
let genRandomNumbers n max =
    let rnd = System.Random()
    List.init n (fun _ -> max * rnd.NextDouble() )

let printDescriptiveStatistics x =
    let fns = MapToSuccess.DescriptiveStatistics.getFiveNumberSummary x
    let spd = MapToSuccess.DescriptiveStatistics.getSpread x
    // Bonus points to whoever knows why I picked these numbers!
    let pctList = [ 12; 34; 67; 78; 83; 97 ]
    let pct = MapToSuccess.DescriptiveStatistics.getArbitraryPercentiles x pctList
    let pctRes = pctList |> List.zip pct

    printfn @"Five-number summary:
    Min    = %f
    25th   = %f
    Median = %f
    75th   = %f
    Max    = %f" fns.Min fns.LowerQuartile fns.Median fns.UpperQuartile fns.Max

    printfn @"Spread variables:
    Var    = %f
    StDev  = %f" spd.Variance spd.StandardDeviation

    printfn "Percentiles:"
    pctRes |> List.map(fun (r, p) -> printfn "    %i     = %f" p r ) |> ignore

// Function call for option 8.
let analyzeWaitStats fileName =
    MapToSuccess.WaitStats.analyzeWaitStats fileName

// Function call for option 9.
let viewWindows fileName =
    MapToSuccess.WaitStats.viewWindows fileName

[<EntryPoint>]
let main argv = 
    printfn @"F# Demo.
    
This is an F# demonstration.  Select one of the following options to demonstrate F# functionality:
     0)  Review basic F# functionality.
     1)  Track movie sales and learn about currying and the pipeline.
     2)  Read World Bank JSON data with a type provider.
     3)  Read an XML query plan with a type provider.
     4)  F# SQL type provider:  querying data.
     5)  F# SQL type provider:  using a stored procedure.
     6)  F# SQL type provider:  writing to a table using a TVP and a stored procedure.
     7)  Generate descriptive statistics for a data set.
     8)  Graph wait stats data with FSharp.Charting.
     9)  View windows of data and calculate moving averages.
    10)  Build a fractal tree."

    Console.Write "Choose the form of your demo:  "
    let i = System.Console.ReadLine()
    Console.WriteLine Environment.NewLine

    match i with
    | "0" -> MapToSuccess.Basics.outputText "This is an important message!"
    | "1" -> findMovieSales "Riddick"
    | "2" -> printWorldBankData "au"
    | "3" -> printMissingIndex "Showplan.xml"
    | "4" -> printAirportDetails "RDU"
    | "5" -> printAirportDetailsSP "CMH"
    | "6" -> writeToSqlWithTvp (); printfn "Check the dbo.DelayByState table for results."
    | "7" -> printDescriptiveStatistics (genRandomNumbers 50 70.)
    | "8" -> analyzeWaitStats "WaitStats.csv" |> ignore
    | "9" -> viewWindows "WaitStats.csv" |> ignore
    | "10" -> MapToSuccess.FractalForest.displayTree 500. 50. 14. |> ignore
    | _ -> printfn "You chose an invalid option.  Better luck next time."

    Console.WriteLine Environment.NewLine

    printfn "Press Enter to exit this application."
    System.Console.ReadLine() |> ignore
    0 // return an integer exit code
