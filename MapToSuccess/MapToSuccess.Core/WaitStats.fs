module MapToSuccess.WaitStats

open System
open FSharp.Data
open XPlot.Plotly
open MathNet.Numerics.Statistics

type WaitStat = CsvProvider<"WaitStats.csv",",",100>

let getWaits (fileName:string) =
    WaitStat.Load(fileName).Rows

let getGroupedWaits (fileName:string) =
    getWaits fileName
        |> Seq.groupBy (fun w -> w.WaitType)
        |> Seq.map(fun (t, wait) -> (t, wait |> Seq.map (fun w -> w.MillisecondsWaiting)) )

let getWaitSequences (fileName:string) =
    let waits = getWaits fileName
    let msWaiting = waits |> Seq.map(fun (w) -> w.MillisecondsWaiting)
    let waitType = waits |> Seq.map(fun (w) -> w.WaitType)
    (msWaiting, waitType)

let analyzeWaitStats (fileName:string) =
    let waits = getWaits fileName
    let groupedWaits = getGroupedWaits fileName

    // Print out basic details on each grouping.
    groupedWaits
    |> Seq.iter (fun (wt, g) -> printfn "%s - %i" wt (g |> Seq.length))

    let boxes = groupedWaits
                |> Seq.map(fun (wt, g) -> Box(y = g, name = wt))
    boxes
    |> Chart.Plot
    |> Chart.Show

    let times = waits
                |> Seq.filter (fun f -> f.WaitType = "CPU")
                |> Seq.map (fun w -> (w.Time) )
    let cpus = waits
                |> Seq.filter (fun f -> f.WaitType = "CPU")
                |> Seq.map (fun w -> (w.MillisecondsWaiting) )

    let cpuWaitLine = Scatter(x = times, y = cpus)
    cpuWaitLine
    |> Chart.Plot
    |> Chart.Show


let viewWindows (fileName:string) =
    let waits = getWaits fileName
    let cpuwaits = waits
                    |> Seq.filter (fun f -> f.WaitType = "CPU")
                    |> Seq.filter (fun f -> f.Time >= DateTimeOffset(DateTime(2018,02,18)))
                    |> Seq.filter (fun f -> f.Time < DateTimeOffset(DateTime(2018,02,19)))
                    |> Seq.map (fun w -> float(w.MillisecondsWaiting) )
    let windows = Seq.windowed 3 cpuwaits
    let movingAvg = Seq.map Array.average windows
    for mv in movingAvg do
        printfn "%A" mv