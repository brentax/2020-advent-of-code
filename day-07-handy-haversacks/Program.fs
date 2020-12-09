// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.IO
open System.Text.RegularExpressions

type Edge = {
    OuterBag: string
    InnerBag: string
    Count: int
}

let readInput =
    File.ReadLines("input")

let bagRegexPattern = " bag(s?)(\\.?)"
let parseLine (line: string) : seq<Edge> =
    let noBagLine = Regex.Replace(line, bagRegexPattern, "")
    let tokens = noBagLine.Split(" contain ")
    let outerBag = tokens.[0]

    if tokens.[1] = "no other" then
        Seq.empty
    else
        let innerBagsUnparsed = tokens.[1].Split(", ")

        innerBagsUnparsed |> Array.map (
                                        fun token -> 
                                            let bagTokens = token.Split(' ', 2)
                                            { 
                                                Edge.OuterBag = outerBag
                                                Edge.InnerBag = bagTokens.[1]
                                                Edge.Count = Int32.Parse(bagTokens.[0])
                                            }
                                        ) |>
                                        Array.toSeq

let rec getUniqueBagsThatContain (innerBag: string) (graph: Map<string, seq<Edge>>) =
    if graph.ContainsKey innerBag then
        graph.[innerBag] |> Seq.fold (fun bagsSoFar edge -> bagsSoFar |> Set.add edge.OuterBag |> Set.union (getUniqueBagsThatContain edge.OuterBag graph)) Set.empty
    else
        Set.empty

let rec countBagsContainedIn (outerBag: string) (graph: Map<string, seq<Edge>>) =
    if graph.ContainsKey outerBag then
        graph.[outerBag] |> Seq.fold (fun sum edge -> sum + edge.Count * (1 + countBagsContainedIn edge.InnerBag graph)) 0
    else
        0

[<EntryPoint>]
let main argv =
    let edges = readInput |> Seq.collect parseLine
    
    let innerToOuterGraph = edges |> Seq.groupBy (fun edge -> edge.InnerBag) |> Map.ofSeq
    let bagCount = getUniqueBagsThatContain "shiny gold" innerToOuterGraph |> Set.count
    printfn "Count of bag colors that can contain one 'shiny gold': %d" bagCount

    let outerToInnerGraph = edges |> Seq.groupBy (fun edge -> edge.OuterBag) |> Map.ofSeq
    let numBagsContained = countBagsContainedIn "shiny gold" outerToInnerGraph
    printfn "Count of bags contained in one 'shiny gold': %d" numBagsContained
    0 // return an integer exit code