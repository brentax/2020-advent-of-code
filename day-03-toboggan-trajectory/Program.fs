// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.IO

let tree = '#'

let readInput =
    File.ReadLines("input")

let multiples n =
    let rec loop i = 
        seq {
            yield i
            yield! loop (i + n)
        }
    seq { yield 0; yield! loop n }

let folder (count: int) (index: int) (row: string) : int =
    if row.[(index % row.Length)] = tree then count + 1 else count

[<EntryPoint>]
let main argv =
    let rows = readInput
    let treeCountRight1 = Seq.fold2 folder 0 (multiples 1) rows
    let treeCountRight3 = Seq.fold2 folder 0 (multiples 3) rows
    let treeCountRight5 = Seq.fold2 folder 0 (multiples 5) rows
    let treeCountRight7 = Seq.fold2 folder 0 (multiples 7) rows
    let everyOtherRow = rows |> Seq.indexed |> Seq.filter(fun (index, _) -> index % 2 = 0) |> Seq.map(fun(_, row) -> row)
    let treeCountDown2 = Seq.fold2 folder 0 (multiples 1) everyOtherRow
    printfn "num trees %d %d %d %d %d" treeCountRight1 treeCountRight3 treeCountRight5 treeCountRight7 treeCountDown2
    printfn "num trees %d" ((int64)treeCountRight1 * (int64)treeCountRight3 * (int64)treeCountRight5 * (int64)treeCountRight7 * (int64)treeCountDown2)
    0 // return an integer exit code