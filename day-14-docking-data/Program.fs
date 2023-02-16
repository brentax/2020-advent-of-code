// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System.IO

type Batch = {
    Mask: string
    Commands: seq<(int * uint64)>
}

let readInput =
    File.ReadLines("input")

let parseInput (lines: seq<string>) : seq<Batch> =
    let folder (state: (seq<Batch> * Batch)) (line: string) =
        let (batches, batchSoFar) = state
        if line.StartsWith("mask") || line = String.empty then
            let mask = line.Substring(7)
            (Seq.append batches (seq { batchSoFar }), { Mask = mask, Commands = Seq.empty })
        else
            let newCommands = Seq.append batchSoFar.Commands (seq { () })
            (batches, { batchSoFar with Commands = batchSoFar.})
    lines |> Seq.fold folder

// Define a function to construct a message to print
let from whom =
    sprintf "from %s" whom

[<EntryPoint>]
let main argv =
    let message = from "F#" // Call the function
    printfn "Hello world %s" message
    0 // return an integer exit code