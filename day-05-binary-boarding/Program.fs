// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.IO

let readInput =
    File.ReadLines("input")

let convertToBinary boardingNumber =
    let replaceChar c =
        match c with
        | 'B' | 'R' -> '1'
        | 'F' | 'L' -> '0'
        | _ -> c
    let binaryString = boardingNumber |> String.map replaceChar
    Convert.ToInt32(binaryString, 2)

[<EntryPoint>]
let main argv =
    let highestBoardingNumber = readInput |> Seq.map convertToBinary |> Seq.sort |> Seq.pairwise |> Seq.filter (fun (a, b) -> b - a = 2)
    printfn "Hello world %A" highestBoardingNumber
    0 // return an integer exit code