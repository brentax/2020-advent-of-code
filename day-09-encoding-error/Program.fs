// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.IO

let readInput =
    File.ReadAllLines("input") |>
        Seq.map Int64.Parse

let isMissingSum (targetSum : int64, numbers : int64[]) : bool =
    let num = numbers |> Array.tryFind(fun x -> numbers |> Array.contains(targetSum - x))

    match num with
    | Some _ -> false // If we found a pair, return false to signal the sum isn't missing
    | None -> true // Else it's missing

let getFirstIncorrectNumber (windowSize: int) (numbers: seq<int64>) =
    let windows = numbers |> Seq.windowed windowSize
    let numbersAfterPreamble = numbers |> Seq.skip windowSize

    windows |> Seq.zip numbersAfterPreamble |> Seq.find isMissingSum |> fst

let getEncryptionWeakness (targetNum: int64) (numbers: int64[]) =
    let rec helper (rearIndex: int) (frontIndex: int) (sum: int64) =
        if sum = targetNum then
            let range = numbers.[rearIndex..frontIndex]
            (range |> Array.min, range |> Array.max) 
        elif sum > targetNum then
            helper (rearIndex + 1) frontIndex (sum - numbers.[rearIndex])
        elif sum < targetNum then
            helper rearIndex (frontIndex + 1) (sum + numbers.[frontIndex+1])
        else
            (0L, 0L)

    let (min, max) = helper 0 1 (numbers.[0] + numbers.[1])
    min + max


[<EntryPoint>]
let main argv =
    let numbers = readInput
    let error = numbers |> getFirstIncorrectNumber 25
    let encryptionWeakness = numbers |> Array.ofSeq |> getEncryptionWeakness error
    printfn "First incorrect number: %d" error
    printfn "Encryption weakness: %A" encryptionWeakness
    0 // return an integer exit code