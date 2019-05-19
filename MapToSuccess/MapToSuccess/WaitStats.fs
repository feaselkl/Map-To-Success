module MapToSuccess.WaitStats

open System
open FSharp.Data
open FSharp.Charting
open MathNet.Numerics.Statistics

type WaitStat = CsvProvider<"WaitStats.csv",",",100>

let getWaits (fileName:string) =
    WaitStat.Load(fileName).Rows

let getGroupedWaits (fileName:string) =
    getWaits fileName
        |> Seq.groupBy (fun w -> w.WaitType)
        |> Seq.map(fun (t, wait) -> (t, wait |> Seq.map (fun w -> w.MillisecondsWaiting)) )

let analyzeWaitStats (fileName:string) =
    let waits = getWaits fileName
    let groupedWaits = getGroupedWaits fileName

    for (t, wait) in groupedWaits do
        printfn "%s - %i" t (wait |> Seq.length)
    
    let boxplot = (groupedWaits |> Chart.BoxPlotFromData).ShowChart()
    System.Windows.Forms.Application.Run(boxplot)

    let cpuwaits = waits
                    |> Seq.filter (fun f -> f.WaitType = "CPU")
                    |> Seq.map (fun w -> (w.Time, w.MillisecondsWaiting) )
    let cpuline = Chart.Line(cpuwaits, Name="CPU waits over time", Title="CPU Waits Over Time", YTitle="Wait Time Per Hour (ms)").ShowChart()
    System.Windows.Forms.Application.Run(cpuline)

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