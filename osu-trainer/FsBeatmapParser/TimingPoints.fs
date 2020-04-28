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

let isTimingPoint            = function TimingPoint _ -> true          | _ -> false
let getTimingPoint           = function TimingPoint x -> x
let getTimingPointBeatLength = function TimingPoint x -> x.beatLength  | _ -> 0M

let isNotTimingPointComment hobj =
    match hobj with
    | Comment _ -> false
    | _ -> true

let removeTimingPointComments (objs:list<TimingPoint>) = (List.filter isNotTimingPointComment objs)
    
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

let timingPointToString tp = 
    match tp with
    | TimingPoint tp -> sprintf "%d,%M,%d,%d,%d,%d,%d,%d" tp.time tp.beatLength tp.meter tp.sampleSet tp.sampleIndex tp.volume (if tp.uninherited then 1 else 0) tp.effects
    | Comment c -> c

let parseTimingPointSection : string list -> TimingPoint list = parseSectionUsing tryParseTimingPoint
