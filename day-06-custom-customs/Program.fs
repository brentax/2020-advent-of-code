// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.IO

let readInput =
    File.ReadLines("input")

let parseInput lines =
    let folder (state: seq<Set<char>> * Set<char>) (line: string) =
        let (allForms, formSoFar) = state
        if line.Trim() = String.Empty then (Seq.append allForms (seq { formSoFar }), Set.empty)
        else 
            let form = line.ToCharArray() |> Set.ofArray
            (allForms, Set.union form formSoFar)
    Seq.fold folder (Seq.empty, Set.empty) lines |> fst

let parseInput2 lines =
    let folder (state: seq<seq<Set<char>>> * seq<Set<char>>) (line: string) =
        let (allForms, formSoFar) = state
        if line.Trim() = String.Empty then (Seq.append allForms (seq { formSoFar }), Seq.empty)
        else 
            let form = line.ToCharArray() |> Set.ofArray
            (allForms, Seq.append formSoFar (seq { form }))
    Seq.fold folder (Seq.empty, Seq.empty) lines |> fst |> Seq.map (Seq.reduce Set.intersect)


[<EntryPoint>]
let main argv =
    let questionCount = readInput |> parseInput |> Seq.map Set.count |> Seq.reduce (+)
    let questionCount2 = readInput |> parseInput2 |> Seq.map Set.count |> Seq.reduce (+)
    printfn "Number of questions answered: %d %d" questionCount questionCount2
    0 // return an integer exit code