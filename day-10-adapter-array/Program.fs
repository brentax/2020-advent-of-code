// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.IO

let readInput =
    File.ReadAllLines("input") |>
        Seq.map Int32.Parse |>
        Seq.sort

let countRunsWithOneDifference (differences: seq<int>) =
    let folder state number =
        let (runs, currentRun) = state
        match number with
        | 1 -> (runs, (currentRun + 1))
        | _ -> ((Seq.append runs (seq { currentRun })), 0)
    
    differences |> Seq.fold folder (Seq.empty, 0) |> fst

[<EntryPoint>]
let main argv =
    let sortedJoltages = readInput |> Seq.append (seq { 0 }) // prepend 0 for the outlet
    let differences = (Seq.append sortedJoltages (seq { 3 })) |> // append 3 for the last jump to the device
                        Seq.pairwise |>
                        Seq.map (fun (x, y) -> y - x)
    let differenceCounts = differences |> Seq.countBy id |> Map.ofSeq
    let answer = differenceCounts.[1] * (differenceCounts.[3])
    printfn "Number of '1' joltage jumps * '3' joltage jumps: %d" answer

    let combinationMap = Map.empty.Add(4, 7L).Add(3, 4L).Add(2, 2L)
    let combinations = differences |> 
                        countRunsWithOneDifference |>
                        Seq.filter (fun x -> x > 1) |>
                        Seq.map (fun runCount -> combinationMap.[runCount]) |> 
                        Seq.reduce (*)
    printfn "Possible combination count: %d" combinations
    0 // return an integer exit code

(* 
When there's a run of numbers with a difference of 1,
we need to include the numbers on either end to maintain
a distance of 3 to the next number outside the run

2 combinations in a run of 3
0 1 2
0   2

4 combinations in a run of 4
0 1 2 3
0   2 3
0     3
0 1   3

7 combinations in a run of 5
0 1 2 3 4
0 1 2   4
0 1     4
0 1   3 4
0     3 4
0   2   4
0   2 3 4


5 5 5 3 4 2 5
7 7 7 2 4 1 7
*)