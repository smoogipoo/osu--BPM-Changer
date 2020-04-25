module HitObjects

open System
open JunUtils

type ObjNoEndTime =
    {
        x          : int;
        y          : int;
        time       : int;
        typeval    : int;
        hitSound   : int;
        remainder  : string; // just don't bother trying to parse everything else
    }

type ObjWithEndTime = 
    {
        x          : int;
        y          : int;
        time       : int;
        typeval    : int;
        endTime    : int;
        hitSound   : int;
        remainder  : string; // just don't bother trying to parse everything else
    }


type HitObject = 
    | HitCircle of ObjNoEndTime
    | Slider    of ObjNoEndTime
    | Spinner   of ObjWithEndTime
    | Hold      of ObjWithEndTime
    | Comment   of string


let tryParseObjNoEndTime vals : ObjNoEndTime option =
    match vals with
    | x::y::ti::ty::hs::rest ->
        if (typesMatch [x;y;ti;ty;hs] ["int";"int";"int";"int";"int"]) then
            Some({
                x           = int x;
                y           = int y;
                time        = int ti;
                typeval     = int ty;
                hitSound    = int hs;
                remainder   = String.Join(",", rest);
            })
        else parseError vals
    | _ -> None
    

let tryParseObjWithEndTime vals : ObjWithEndTime option =
    match vals with
    | x::y::ti::ty::hs::et::rest ->
        if (typesMatch [x;y;ti;ty;hs;et] ["int";"int";"int";"int";"int";"int"]) then
            Some({
                x         = int x;
                y         = int y;
                time      = int ti;
                typeval   = int ty;
                endTime   = int et;
                hitSound  = int hs;
                remainder = String.Join(",", rest);
            })
        else parseError vals
    | _ -> None
    

let tryParseHitObject line : HitObject option =
    let vals = parseCsv line 
    match vals with
    | _::_::_::typeval::rest -> 
        match int typeval with

        // bit 0 high => HitCircle
        | typebyte when (typebyte &&& 1) <> 0 ->
            match tryParseObjNoEndTime vals with
            | Some(obj) -> Some(HitCircle(obj))
            | None -> Some(Comment(line))
        
        // bit 1 high => HitCircle
        | typebyte when (typebyte &&& 2) <> 0 ->
            match tryParseObjNoEndTime vals with
            | Some(obj) -> Some(Slider(obj))
            | None -> Some(Comment(line))
        
        // bit 3 high => Spinner
        | typebyte when (typebyte &&& 8) <> 0 ->
            match tryParseObjWithEndTime vals with
            | Some(obj) -> Some(Spinner(obj))
            | None -> Some(Comment(line))

        // bit 7 high => osu mania hold
        | typebyte when (typebyte &&& 128) <> 0 ->
            match tryParseObjWithEndTime vals with
            | Some(obj) -> Some(Hold(obj))
            | None -> Some(Comment(line))

        | _ -> Some(Comment(line))
    | _ -> Some(Comment(line))

let parseHitObjectSection = parseSectionUsing tryParseHitObject
