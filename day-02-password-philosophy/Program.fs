// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.IO

type Range = {
    Low: int
    High: int
}

type Password = {
    Range: Range
    Character: char
    Value: string
}

let readInput =
    let lines = File.ReadLines("input")

    let stringToPassword (line: string) =
        let tokens = line.Split(" ")
        let rangeTokens = tokens.[0].Split("-")
        let charToken = tokens.[1].Split(":")
        let valueToken = tokens.[2]

        {
            Password.Range = { 
                Range.Low = Int32.Parse(rangeTokens.[0])
                Range.High = Int32.Parse(rangeTokens.[1])
            }
            Password.Character = charToken.[0].ToCharArray().[0]
            Password.Value = valueToken
        }

    lines |> Seq.map(stringToPassword)

let isPasswordValid (pw: Password) =
    let charCount = pw.Value |> String.filter(fun c -> c = pw.Character) |> String.length
    charCount >= pw.Range.Low && charCount <= pw.Range.High

let isPasswordValid2(pw: Password) =
    (pw.Value.[pw.Range.Low - 1] = pw.Character) <> (pw.Value.[pw.Range.High - 1] = pw.Character)

[<EntryPoint>]
let main argv =
    let passwords = readInput
    let count = passwords |> Seq.filter isPasswordValid |> Seq.length
    let count2 = passwords |> Seq.filter isPasswordValid2 |> Seq.length
    printfn "%d %d" count count2
    0 // return an integer exit code