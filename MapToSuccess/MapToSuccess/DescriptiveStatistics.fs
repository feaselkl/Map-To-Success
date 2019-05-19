module MapToSuccess.DescriptiveStatistics

open MathNet.Numerics.Statistics

type FiveNumberSummary = {
    Min:float
    LowerQuartile:float
    Median:float
    UpperQuartile:float
    Max:float
}

type Spread = {
    StandardDeviation:float
    Variance:float
}

let getFiveNumberSummary (x:List<float>) =
    let res = Statistics.FiveNumberSummary x
    { Min = res.[0]; LowerQuartile = res.[1]; Median = res.[2]; UpperQuartile = res.[3]; Max = res.[4] }

let getArbitraryPercentiles (x:List<float>) (pct:List<int>) =
    pct |> List.map (fun p -> Statistics.Percentile (x, p) )

let getSpread (x:List<float>) =
    { StandardDeviation = Statistics.StandardDeviation x; Variance = Statistics.Variance x }