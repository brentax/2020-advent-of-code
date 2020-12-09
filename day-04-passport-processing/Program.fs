// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.IO
open System.Text.RegularExpressions

let readInput =
    File.ReadLines("input")

let mergeMaps map1 map2 =
    Map.fold (fun acc key value -> Map.add key value acc) map1 map2

let keySet (map) =
    map |> Map.toSeq |> Seq.map fst |> Set.ofSeq

let byrValidator byr =
    let birthyear = Int32.Parse byr
    (birthyear >= 1920) && (birthyear <= 2002)

let iyrValidator iyr =
    let issueYear = Int32.Parse iyr
    (issueYear >= 2010) && (issueYear <= 2020)

let eyrValidator eyr =
    let expirationYear = Int32.Parse eyr
    (expirationYear >= 2020) && (expirationYear <= 2030)

let hgtValidator (hgt: string) =
    if hgt.Length <= 2 then false else
    let unit = hgt.Substring (hgt.Length - 2)
    let value = hgt.Substring(0, (hgt.Length - 2))
    printfn "%s %s" value unit
    let height = Int32.Parse value
    ((unit = "cm") && (height >= 150) && (height <= 193)) ||
        ((unit = "in") && (height >= 59) && (height <= 76))

let hclValidator (hcl: string) =
    let pattern = "^#[a-f0-9]{6}$"
    Regex.IsMatch(hcl, pattern)

let validEyeColors = Set.ofList ["amb"; "blu"; "brn"; "gry"; "grn"; "hzl"; "oth"]
let eclValidator ecl =
    ecl |> validEyeColors.Contains

let pidValidator pid =
    let pattern = "^\\d{9}$"
    Regex.IsMatch(pid, pattern)


let parseInput lines =
    let folder (state: seq<Map<string, string>> * Map<string, string>) (line: string) =
        let (passports, passportSoFar) = state
        if line.Trim() = String.Empty then (Seq.append passports (seq { passportSoFar }), Map.empty)
        else 
            let tokens = line.Split " "
            let newFields = tokens |>
                                Array.map(fun token -> 
                                                let keyValue = token.Split(":")
                                                (keyValue.[0], keyValue.[1])
                                ) |>
                                Map.ofArray
            (passports, mergeMaps passportSoFar newFields)
    Seq.fold folder (Seq.empty, Map.empty) lines

[<EntryPoint>]
let main argv =
    let validators = Map.empty.Add("byr", byrValidator).Add("iyr", iyrValidator).Add("eyr", eyrValidator).Add("hgt", hgtValidator).Add("hcl", hclValidator).Add("ecl", eclValidator).Add("pid", pidValidator)
    let requiredFields = validators |> keySet

    let validate (passport: Map<string, string>) =
        //let allFieldsPresent = passport |> keySet |> Set.isSubset requiredFields
        let numValidFields = validators |>
                                Map.filter(fun fieldName validator -> passport.ContainsKey(fieldName) && validator passport.[fieldName]) |>
                                Map.count
        numValidFields = validators.Count

    let rawInput = readInput
    let (passports, _) = rawInput |> parseInput

    let validPasswordCount = passports |>
                                Seq.map keySet |>
                                Seq.filter (Set.isSubset requiredFields) |>
                                Seq.length
    
    let validPasswordCount2 = passports |>
                                Seq.filter validate |>
                                Seq.length
    printfn "Valid password count: %d %d" validPasswordCount validPasswordCount2
    0 // return an integer exit code