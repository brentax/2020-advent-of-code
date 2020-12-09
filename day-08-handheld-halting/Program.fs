// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.IO

type CodeLine = {
    Instruction: string
    Value: int
}

let readInput =
    File.ReadLines("input")

let parseLine (line: string) : CodeLine =
    let tokens = line.Split(" ")

    {
        CodeLine.Instruction = tokens.[0]
        CodeLine.Value = Int32.Parse(tokens.[1])
    }

let evaluateProgram (code: CodeLine[]) : int =
    let rec evaluateProgramHelper (acc: int) (index: int) (visitedInstructions: Set<int>) =
        if visitedInstructions.Contains index then
            acc
        else
            let currentLine = code.[index]
            match currentLine.Instruction with
            | "nop" -> evaluateProgramHelper acc (index + 1) (visitedInstructions.Add index)
            | "acc" -> evaluateProgramHelper (acc + currentLine.Value) (index + 1) (visitedInstructions.Add index)
            | "jmp" -> evaluateProgramHelper acc (index + currentLine.Value) (visitedInstructions.Add index)
    
    evaluateProgramHelper 0 0 Set.empty

let evaluateProgramWithAutocorrect (code: CodeLine[]) : Result<int, int> =
    let rec evaluateProgramHelper (acc: int) (index: int) (visitedInstructions: Set<int>) (hasCorrection: bool)=
        if visitedInstructions.Contains index then
            Error acc
        elif index = code.Length then
            Ok acc
        elif index < 0 || index > code.Length then
            Error acc
        else
            let currentLine = code.[index]
            match currentLine.Instruction with
            | "acc" -> evaluateProgramHelper (acc + currentLine.Value) (index + 1) (visitedInstructions.Add index) hasCorrection
            | "jmp" -> 
                if hasCorrection then
                    evaluateProgramHelper acc (index + currentLine.Value) (visitedInstructions.Add index) hasCorrection
                else
                    let withCorrection = evaluateProgramHelper acc (index + 1) (visitedInstructions.Add index) true
                    match withCorrection with
                    | Ok value -> Ok value
                    | Error _ -> evaluateProgramHelper acc (index + currentLine.Value) (visitedInstructions.Add index) false
            | "nop" -> 
                if hasCorrection then
                    evaluateProgramHelper acc (index + 1) (visitedInstructions.Add index) hasCorrection
                else
                    let withCorrection = evaluateProgramHelper acc (index + currentLine.Value) (visitedInstructions.Add index) true
                    match withCorrection with
                    | Ok value -> Ok value
                    | Error _ -> evaluateProgramHelper acc (index + 1) (visitedInstructions.Add index) false
    
    evaluateProgramHelper 0 0 Set.empty false

[<EntryPoint>]
let main argv =
    let answer = readInput |> Seq.map parseLine |> Array.ofSeq |> evaluateProgram
    
    printfn "Accumulator value at time of loop: %d" answer

    let answerCorrected = readInput |> Seq.map parseLine |> Array.ofSeq |> evaluateProgramWithAutocorrect
    
    printfn "Accumulator value with correction: %A" answerCorrected
    0 // return an integer exit code