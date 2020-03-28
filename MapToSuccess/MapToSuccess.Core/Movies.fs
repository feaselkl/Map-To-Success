module MapToSuccess.Movies

open System

type MpaaRating =
    | G
    | PG
    | PG13
    | R
    | NC17

type Movie = { Title : string; Year : int; Rating : MpaaRating option }

type MovieSales = {
    Movie : Movie
    Year : int
    Week : int
    Sales : int
}

let sumMovieSales movie movieSales =
    movieSales
    |> Seq.filter (fun m -> m.Movie = movie)
    |> Seq.sumBy (fun m -> m.Sales)

let buildSampleMovies =
    let movies = [  { Title = "The Last Witch Hunter"; Year = 2014; Rating = None }
                    { Title = "Riddick"; Year = 2013; Rating = Some(R) }
                    { Title = "Fast Five"; Year = 2011; Rating = Some(PG13) }
                    { Title = "Babylon A.D."; Year = 2008; Rating = Some(PG13) } ]
    movies

let generateSampleSales minSales maxSales =
    let r = Random(17)
    r.Next(minSales, maxSales)

let buildMovieSalesRow movie year week =
    let minSales, maxSales = 0, 150000
    let sales = generateSampleSales minSales maxSales
    { Movie = movie; Year = year; Week = week; Sales = sales }

let buildWeeklySampleMovieSalesData movie year =
    let weeks = [1..52]
    weeks
    |> List.map (buildMovieSalesRow movie year)

let buildSampleMovieSalesData movie =
    let years = [2018; 2019]
    years
    |> List.collect (buildWeeklySampleMovieSalesData movie)

let buildSampleMovieSales movies =
    movies
    //If you are familiar with Scala, collect is flatMap
    |> List.collect buildSampleMovieSalesData
