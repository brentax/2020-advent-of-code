// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.IO
open System.Diagnostics

let readInput =
    let lines = File.ReadAllLines("input") |> Seq.toList            
    lines |> List.map(Int32.Parse)

let findNumbersThatSumTo (numbers : list<int>) (targetSum : int) : (int * int) option =
    let num1 = numbers |> List.tryFind(fun x -> numbers |> List.contains(targetSum - x))
    num1 |> Option.map(fun x -> (x, targetSum - x))

let findNumbersThatSumToRecursive (numbers : list<int>) (targetSum : int) : (int * int) option =
    let rec helper (ascending : list<int>) (descending : list<int>) =
        if ascending.IsEmpty || descending.IsEmpty || ascending.Head > descending.Head then None
        else if ascending.Head + descending.Head = targetSum then Some(ascending.Head, descending.Head)
        else if ascending.Head + descending.Head < targetSum then helper ascending.Tail descending
        else if ascending.Head + descending.Head > targetSum then helper ascending descending.Tail
        else None
    
    let asc = numbers |> List.sort
    let desc = asc |> List.rev
    
    helper asc desc

let findThirdNumber (numbers: list<int>) (targetSum : int) twoNumFun : (int * int * int) =
    let mapper acc item =
        match acc with
        | Some(a, b, c) -> None, Some(a, b, c)
        | None -> None, ((twoNumFun numbers (targetSum - item)) |> Option.map(fun (c, d) -> (c, d, item)))

    let _, result = numbers |> List.mapFold mapper None
    result.Value

[<EntryPoint>]
let main argv =    
    let numbers = readInput
    let target = 2020

    let stopwatch = Stopwatch()
    stopwatch.Start()
    let triplet = findThirdNumber numbers target findNumbersThatSumTo
    stopwatch.Stop()
    triplet |> fun (a, b, c) -> (a * b * c) |> printfn "%d"
    printfn "Calculated in %d milliseconds, brute force" stopwatch.ElapsedMilliseconds

    stopwatch.Restart()
    let tripletRec = findThirdNumber numbers target findNumbersThatSumToRecursive
    stopwatch.Stop()
    tripletRec |> fun (a, b, c) -> (a * b * c) |> printfn "%d"
    printfn "Calculated in %d milliseconds, recursively" stopwatch.ElapsedMilliseconds
    0 // return an integer exit code