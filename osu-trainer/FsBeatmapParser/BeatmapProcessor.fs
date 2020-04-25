module BeatmapParser

open System.IO
open JunUtils
open General
open Editor
open Metadata
open Difficulty
open Events
open TimingPoints
open Colours
open HitObjects

type Section = 
    | General      of GeneralInfo list
    | Editor       of EditorSetting list
    | Metadata     of MetadataInfo list
    | Difficulty   of DifficultySetting list
    | Events       of BeatmapEvent list
    | TimingPoints of TimingPoint list
    | Colours      of ColourSetting list
    | HitObjects   of HitObject list
let getGeneralList      = function General x -> x      | _ -> []
let getEditorList       = function Editor x -> x       | _ -> []
let getMetadataList     = function Metadata x -> x     | _ -> []
let getDifficultyList   = function Difficulty x -> x   | _ -> []
let getEventsList       = function Events x -> x       | _ -> []
let getTimingPointsList = function TimingPoints x -> x | _ -> []
let getColoursList      = function Colours x -> x      | _ -> []
let getHitObjectsList   = function HitObjects x -> x   | _ -> []

let isSectionHeader line =
    match line with
    | Regex "^\[(.+)\]" [header] ->
        match header with
        | "General"      -> true
        | "Editor"       -> true
        | "Metadata"     -> true
        | "Difficulty"   -> true
        | "Events"       -> true
        | "TimingPoints" -> true
        | "Colours"      -> true
        | "HitObjects"   -> true
        | _ -> false
    | _ -> false


let splitSections (fileLines:list<string>) : list<list<string>> =

    // get list of line numbers where a section header is placed
    let mutable headerIndices = []
    for i = 0 to (fileLines.Length - 1) do
        if isSectionHeader fileLines.[i] then
            headerIndices <- headerIndices @ [i]

    let rec getSections (sectionDividers:list<int>) (remainingLines:list<string>) : list<list<string>> =
        match sectionDividers with
        | sd1::sd2::sds ->
            remainingLines.[sd1..sd2-1] :: (getSections (sd2::sds) remainingLines)
        | [lastsd] -> [remainingLines.[lastsd..]]
        | _ -> [] // file contains no sections headers?

    getSections headerIndices fileLines


let parseSection (sectionLines:list<string>) : Section =
    assert (sectionLines.Length > 0)
    match sectionLines.[0] with 
    | "[General]"      -> General      (parseGeneralSection     (sectionLines.[1..]))
    | "[Editor]"       -> Editor       (parseEditorSection      (sectionLines.[1..]))
    | "[Metadata]"     -> Metadata     (parseMetadataSection    (sectionLines.[1..]))
    | "[Difficulty]"   -> Difficulty   (parseDifficultySection  (sectionLines.[1..]))
    | "[Events]"       -> Events       (parseEventSection       (sectionLines.[1..]))
    | "[TimingPoints]" -> TimingPoints (parseTimingPointSection (sectionLines.[1..]))
    | "[Colours]"      -> Colours      (parseColourSection      (sectionLines.[1..]))
    | "[HitObjects]"   -> HitObjects   (parseHitObjectSection   (sectionLines.[1..]))
    | _ -> assert false; General([]) // should never happen


let parseSectionsToMap (sections: list<list<string>> ) : Map<string, Section> =

    let headerIs headerName (section: string list ) =
        section.[0] = (sprintf "[%s]" headerName)

    [
        "General",      sections |> List.filter (headerIs "General"     ) |> List.concat |> parseSection;
        "Editor",       sections |> List.filter (headerIs "Editor"      ) |> List.concat |> parseSection;
        "Metadata",     sections |> List.filter (headerIs "Metadata"    ) |> List.concat |> parseSection;
        "Difficulty",   sections |> List.filter (headerIs "Difficulty"  ) |> List.concat |> parseSection;
        "Events",       sections |> List.filter (headerIs "Events"      ) |> List.concat |> parseSection;
        "TimingPoints", sections |> List.filter (headerIs "TimingPoints") |> List.concat |> parseSection;
        "Colours",      sections |> List.filter (headerIs "Colours"     ) |> List.concat |> parseSection;
        "HitObjects",   sections |> List.filter (headerIs "HitObjects"  ) |> List.concat |> parseSection;
    ] |> Map.ofList


let parseBeatmapFile filename : Map<string, Section> =
    File.ReadAllLines filename
    |> Array.toList
    |> splitSections
    |> parseSectionsToMap
