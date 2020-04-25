module TimingPoints

open JunUtils


type Tp =
    {
        time        : int;
        beatLength  : decimal;
        meter       : int;
        sampleSet   : int;
        sampleIndex : int;
        volume      : int;
        uninherited : bool;
        effects     : int;
    }

type TimingPoint = 
    | TimingPoint of Tp
    | Comment of string

// timing point syntax:
// time,beatLength,meter,sampleSet,sampleIndex,volume,uninherited,effects
let tryParseTimingPoint line : TimingPoint option = 
    let values = parseCsv line
    if (typesMatch values ["int"; "decimal"; "int"; "int"; "int"; "int"; "bool"; "int"]) then
        match values with
        | [t; bl; m; ss; si; v; ui; fx] ->
            Some(TimingPoint({
                time        = int t;
                beatLength  = decimal bl;
                meter       = int m;
                sampleSet   = int ss;
                sampleIndex = int si;
                volume      = int v;
                uninherited = toBool ui;
                effects     = int fx;
            }))
        | _ -> Some(Comment(line))
    else Some(Comment(line))

let parseTimingPointSection = parseSectionUsing tryParseTimingPoint
