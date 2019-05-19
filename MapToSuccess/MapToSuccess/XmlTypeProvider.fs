module MapToSuccess.XmlTypeProvider

open FSharp.Data

type QueryPlan = XmlProvider<"./Showplan.xml">
type MissingIndex = {
    DatabaseName:string
    SchemaName:string
    TableName:string
    Columns:string
    IncludedColumns:string
}

let returnMissingIndex (queryPlanLocation:string) =
    // Note:  we can load from a different location. The above just gives the type provider the data shape.
    let queryPlanData = QueryPlan.Load(queryPlanLocation)
    let missingIndex = queryPlanData.Batch.Statements.StmtSimple.QueryPlan.MissingIndexes.MissingIndexGroup.MissingIndex

    let columns = missingIndex.ColumnGroups
                    |> Seq.filter(fun cg -> cg.Usage = "EQUALITY" || cg.Usage = "INEQUALITY")
                    |> Seq.collect (fun c -> c.Columns)
                    |> Seq.map (fun c -> c.Name)
                    |> String.concat ","

    let includedColumns = missingIndex.ColumnGroups
                            |> Seq.filter (fun cg -> cg.Usage = "INCLUDE")
                            |> Seq.collect (fun c -> c.Columns)
                            |> Seq.map (fun c -> c.Name)
                            |> String.concat ","

    { DatabaseName = missingIndex.Database
      SchemaName = missingIndex.Schema
      TableName = missingIndex.Table
      Columns = columns
      IncludedColumns = includedColumns
    }
