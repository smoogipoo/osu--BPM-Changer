module Colours

open JunUtils

type Colour =
    {
        r : int;
        g : int;
        b : int;
    }

type ColourSetting =
    | Combo               of int * Colour
    | SliderTrackOverride of Colour
    | SliderBorder        of Colour
    | Comment             of string

let tryParseColour str : Colour option =
    match tryParseCsvInt str with
    | Some(nums) -> 
        match nums with
        | [r; g; b] -> 
            Some({
                r = r;
                g = g;
                b = b;
            })
        | _ -> parseError str
    | _ -> parseError str

let tryParseColourOption line : ColourSetting option =
    match line with
    | Regex @"(.+?)\s?:\s?(.*)" [key; value] ->
        match key with

        // Combo Colour
        | Regex @"^Combo(\d+)$" [comboNumber] ->
            match tryParseColour value with
            | Some(col) -> Some(Combo(int comboNumber, col))
            | _ -> Some(Comment(line))

        // SliderTrackOverride
        | "SliderTrackOverride" ->
            match tryParseColour value with
            | Some(col) -> Some(SliderTrackOverride(col))
            | _ -> Some(Comment(line))

        // SliderBorder
        | "SliderTrackOverride" ->
            match tryParseColour value with
            | Some(col) -> Some(SliderBorder(col))
            | _ -> Some(Comment(line))

        | _ -> parseError line
    | _ -> Some(Comment(line))

let parseColourSection = parseSectionUsing tryParseColourOption
