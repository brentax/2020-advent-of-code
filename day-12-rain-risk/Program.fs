// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.IO

type FerryState = {
    X: float
    Y: float
    Heading: float
}

type FerryWaypointState = {
    FerryX: float
    FerryY: float
    WaypointX: float
    WaypointY: float
}

let readInput =
    File.ReadAllLines("input") |>
        Seq.map (fun line -> (line.Substring(0, 1), Double.Parse(line.Substring(1))))

let moveForward (state: FerryState) (distance: float) : FerryState =
    let headingRad = Math.PI / 180.0 * state.Heading
    let deltaX = distance * (Math.Cos headingRad)
    let deltaY = distance * (Math.Sin  headingRad)

    { state with X = state.X + deltaX; Y = state.Y + deltaY }

let executeCommand (state: FerryState) (command: string * float) : FerryState =
    let (action, value) = command
    match (action) with
    | "N" -> { state with Y = state.Y + value }
    | "S" -> { state with Y = state.Y - value }
    | "E" -> { state with X = state.X + value }
    | "W" -> { state with X = state.X - value }
    | "L" -> { state with Heading = state.Heading + value }
    | "R" -> { state with Heading = state.Heading - value }
    | "F" -> moveForward state value

let rotateWaypoint (state: FerryWaypointState) (angle: float) : FerryWaypointState =
    let angleRad = Math.PI / 180.0 * angle
    let xPrime = state.WaypointX * (Math.Cos angleRad) - state.WaypointY * (Math.Sin angleRad)
    let yPrime = state.WaypointX * (Math.Sin angleRad) + state.WaypointY * (Math.Cos angleRad)

    { state with WaypointX = xPrime; WaypointY = yPrime }

let executeWaypointCommand (state: FerryWaypointState) (command: string * float) : FerryWaypointState =
    let (action, value) = command
    match (action) with
    | "N" -> { state with WaypointY = state.WaypointY + value }
    | "S" -> { state with WaypointY = state.WaypointY - value }
    | "E" -> { state with WaypointX = state.WaypointX + value }
    | "W" -> { state with WaypointX = state.WaypointX - value }
    | "L" -> rotateWaypoint state value
    | "R" -> rotateWaypoint state (-1.0 * value)
    | "F" -> { state with FerryX = state.FerryX + value * state.WaypointX;  FerryY = state.FerryY + value * state.WaypointY; }

[<EntryPoint>]
let main argv =
    let commands = readInput
    // let finalState = commands |> Seq.fold executeCommand { X = 0.0; Y = 0.0; Heading = 0.0 }
    let manhattanDistance = (Math.Abs finalState.X) + (Math.Abs finalState.Y)
    printfn "Manhattan distance travelled: %f" manhattanDistance

    let finalState2 = commands |> Seq.fold executeWaypointCommand { FerryX = 0.0; FerryY = 0.0; WaypointX = 10.0 ; WaypointY = 1.0 }
    let manhattanDistance2 = (Math.Abs finalState2.FerryX) + (Math.Abs finalState2.FerryY)
    printfn "Manhattan distance travelled: %f" manhattanDistance2
    0 // return an integer exit code