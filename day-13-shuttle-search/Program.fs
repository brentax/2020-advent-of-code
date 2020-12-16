// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.IO

let readInput =
    File.ReadLines("input")

[<EntryPoint>]
let main argv =
    let input = readInput
    let departureTime = input |> Seq.head |> Int32.Parse
    let busSchedule = (input |> Seq.tail |> Seq.head).Split(",") |> Array.filter (fun x -> x <> "x") |> Array.map Int32.Parse

    let nextBus = busSchedule |> Seq.map (fun bus -> (bus, bus - departureTime % bus)) |> Seq.minBy snd

    printfn "Next bus: %A, answer: %d" nextBus ((nextBus |> fst) * (nextBus |> snd))
    0 // return an integer exit code


    // t % 7 = 0
    // (t + 1) % 7 = 0
    // (t + 4) % 59 = 0
    // (t + 6) % 31 = 0
    // (t + 7) % 19 = 0