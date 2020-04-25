namespace FsBeatmap

open JunUtils
open BeatmapParser
open General
open Editor
open Metadata
open Difficulty
open Events
open TimingPoints
open Colours
open HitObjects

// C# Facing Interface 

(* Methods

    Beatmap(string)
    static Beatmap Copy()
    void Save()

    void ModifyTiming(decimal multiplier)
    decimal GetBpm()
    (decimal, decimal) GetBpmRange()

*)

(* Properties : only expose those that will be read or modified
    
    General
        Filename
        AudioFilename
        Mode

    Metadata
        Title
        TitleUnicode
        Artist
        ArtistUnicode
        Creator
        Version
        Source
        Tags
        BeatmapID
        BeatmapSetID

    Difficulty
        Hp
        Cs
        Ar
        Od
        SliderTickRate

    Events
        Background

    TimingPoints
        (do not expose)

    Colours
        (do not expose)

    HitObjects
        (do not expose)
*)


[<Class; Sealed>]
type Beatmap(file: string) =
    let internalRepresentation = parseBeatmapFile file
    
    member val public Filename = file with get, set

    member public this.AudioFilename
        with get() =
            match internalRepresentation.["General"] |> getGeneralList |> List.tryFind isAudioFilename with
            | Some x -> getAudioFilename x
            | None -> ""
        and set(a:string) =
            ()

    member public this.Mode
        with get() =
            match internalRepresentation.["General"] |> getGeneralList |> List.tryFind isMode with
            | Some x -> getMode x
            | None -> 0

    member public this.Title
        with get() =
            match internalRepresentation.["Metadata"] |> getMetadataList |> List.tryFind isTitle with
            | Some x -> getTitle x
            | None -> ""
        and set(t:string) =
            ()

    member public this.TitleUnicode
        with get() =
            match internalRepresentation.["Metadata"] |> getMetadataList |> List.tryFind isTitleUnicode with
            | Some x -> getTitleUnicode x
            | None -> ""

        and set(t:string) =
            ()

    member public this.Artist
        with get() =
            match internalRepresentation.["Metadata"] |> getMetadataList |> List.tryFind isArtist with
            | Some x -> getArtist x
            | None -> ""

        and set(a:string) =
            ()

    member public this.ArtistUnicode
        with get() =
            match internalRepresentation.["Metadata"] |> getMetadataList |> List.tryFind isArtistUnicode with
            | Some x -> getArtistUnicode x
            | None -> ""

        and set(a:string) =
            ()

    member public this.Creator
        with get() =
            match internalRepresentation.["Metadata"] |> getMetadataList |> List.tryFind isCreator with
            | Some x -> getCreator x
            | None -> ""

        and set(c:string) =
            ()

    member public this.Version
        with get() =
            match internalRepresentation.["Metadata"] |> getMetadataList |> List.tryFind isVersion with
            | Some x -> getVersion x
            | None -> ""

        and set(v:string) =
            ()

    member public this.Source
        with get() =
            match internalRepresentation.["Metadata"] |> getMetadataList |> List.tryFind isSource with
            | Some x -> getSource x
            | None -> ""

        and set(s:string) =
            ()

    member public this.Tags
        with get() =
            match internalRepresentation.["Metadata"] |> getMetadataList |> List.tryFind isSearchTerms with
            | Some x -> getSearchTerms x
            | None -> []

        and set(t:list<string>) =
            ()

    member public this.BetamapID
        with get() =
            match internalRepresentation.["Metadata"] |> getMetadataList |> List.tryFind isBeatmapID with
            | Some x -> getBeatmapID x
            | None -> 0

        and set(b:int) =
            ()

    member public this.BeatmapSetID
        with get() =
            match internalRepresentation.["Metadata"] |> getMetadataList |> List.tryFind isBeatmapSetID with
            | Some x -> getBeatmapSetID x
            | None -> 0
        and set(b:int) =
            ()

    member public this.Hp
        with get() =
            match internalRepresentation.["Difficulty"] |> getDifficultyList |> List.tryFind isHPDrainRate with
            | Some x -> getHPDrainRate x
            | None -> 0M
        and set(hp:decimal) =
            ()

    member public this.Cs
        with get() =
            match internalRepresentation.["Difficulty"] |> getDifficultyList |> List.tryFind isCircleSize with
            | Some x -> getCircleSize x
            | None -> 0M
        and set(cs:decimal) =
            ()

    member public this.Ar
        with get() =
            match internalRepresentation.["Difficulty"] |> getDifficultyList |> List.tryFind isApproachRate with
            | Some x -> getApproachRate x
            | None -> 0M
        and set(ar:decimal) =
            ()

    member public this.Od
        with get() =
            match internalRepresentation.["Difficulty"] |> getDifficultyList |> List.tryFind isOverallDifficulty with
            | Some x -> getOverallDifficulty x
            | None -> 0M
        and set(od:decimal) =
            ()

    member public this.SliderTickRate
        with get() =
            match internalRepresentation.["Difficulty"] |> getDifficultyList |> List.tryFind isSliderTickRate with
            | Some x -> getSliderTickRate x
            | None -> 0M
        and set(s:decimal) =
            ()


