// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System.IO

let readInput =
    File.ReadAllLines("input") |>
        Seq.map (fun line -> line.ToCharArray()) |>
        Array.ofSeq

let neighborOptions = seq {(-1, -1); (-1, 0); (-1, 1); (0, -1); (0, 1); (1, -1); (1, 0); (1, 1)}

let getContents (grid: char[][]) (coordinates: int * int) =
    let (row, col) = coordinates
    if row < 0 || col < 0 || row >= grid.Length || col >= grid.[0].Length then
        None
    else
        Some grid.[row].[col]

let countOccupiedNeighbors (coordinates: int * int) (grid: char[][]) =
    let (row, col) = coordinates
    neighborOptions |>
        Seq.map (fun (deltaRow, deltaCol) -> (row + deltaRow, col + deltaCol)) |>
        Seq.choose (getContents grid) |>
        Seq.filter (fun value -> value = '#') |>
        Seq.length

let rec isAnySeatInThisDirectionOccupied (coordinates: int * int) (grid: char[][]) (translation: int * int) =
    let (row, col) = coordinates
    let (deltaRow, deltaCol) = translation
    let nextRow = row + deltaRow
    let nextCol = col + deltaCol
    let nextVal = getContents grid (nextRow, nextCol)

    match nextVal with
    | None -> false
    | Some 'L' -> false
    | Some '#' -> true
    | _ -> isAnySeatInThisDirectionOccupied (nextRow, nextCol) grid translation

let countOccupiedNeighborsPart2 (coordinates: int * int) (grid: char[][]) =
    neighborOptions |> Seq.filter (isAnySeatInThisDirectionOccupied coordinates grid) |> Seq.length

let decideNextOccupancyPart2 (grid: char[][]) (coordinates: int * int) =
    let (row, col) = coordinates
    let currentValue = grid.[row].[col]

    if currentValue = '.' then
        '.'
    else
        let occupiedNeighborCount = countOccupiedNeighborsPart2 coordinates grid
        match (currentValue, occupiedNeighborCount) with
        | (currentValue, count) when currentValue = 'L' && count = 0 -> '#'
        | (currentValue, count) when currentValue = '#' && count >= 5 -> 'L'
        | _ -> currentValue

let decideNextOccupancy (grid: char[][]) (coordinates: int * int) =
    let (row, col) = coordinates
    let currentValue = grid.[row].[col]

    if currentValue = '.' then
        '.'
    else
        let occupiedNeighborCount = countOccupiedNeighbors coordinates grid
        match (currentValue, occupiedNeighborCount) with
        | (currentValue, count) when currentValue = 'L' && count = 0 -> '#'
        | (currentValue, count) when currentValue = '#' && count >= 4 -> 'L'
        | _ -> currentValue

let getNextLayoutPart2 (grid: char[][]) =
    grid |> Array.mapi (fun rowIndex row ->
        row |> Array.mapi (fun colIndex _ ->
            decideNextOccupancyPart2 grid (rowIndex, colIndex)
        )
    )

let getNextLayout (grid: char[][]) =
    grid |> Array.mapi (fun rowIndex row ->
        row |> Array.mapi (fun colIndex _ ->
            decideNextOccupancy grid (rowIndex, colIndex)
        )
    )

[<EntryPoint>]
let main argv =
    let mutable layoutA = readInput
    let mutable layoutB = layoutA |> getNextLayout
    
    while layoutA <> layoutB do
        layoutA <- layoutB
        layoutB <- getNextLayout layoutA

    let count = layoutB |> Array.collect id |> Array.countBy id |> Map.ofArray |> Map.find '#'
    
    printfn "Occupied seats, part 1: %d" count

    let mutable layoutA = readInput
    let mutable layoutB = layoutA |> getNextLayoutPart2
    while layoutA <> layoutB do
        layoutA <- layoutB
        layoutB <- getNextLayoutPart2 layoutA
        // generationCount <- generationCount + 1

    let countPart2 = layoutB |> Array.collect id |> Array.countBy id |> Map.ofArray |> Map.find '#'
    printfn "Occupied seats, part 2: %d" countPart2
    
    0 // return an integer exit code