module MapToSuccess.JsonTypeProvider

open FSharp.Data

type WorldBank = JsonProvider<"./WorldBank.json", EmbeddedResource="MapToSuccess, WorldBank.json">
type GovernmentDebt = {
    CountryName:string
    Year:int
    PercentDebt:Option<decimal>
}

let getGovernmentDebt countryCode =
    //In case of Internet emergency, break glass:
    //let wbUri =  "Australia.json"
    let wbUri =  "http://api.worldbank.org/country/" + countryCode + "/indicator/GC.DOD.TOTL.GD.ZS?format=json"
    let doc = WorldBank.Load(wbUri)
    doc.Array
     |> Seq.map (fun f -> { CountryName = f.Country.Value; Year = f.Date; PercentDebt = f.Value })
