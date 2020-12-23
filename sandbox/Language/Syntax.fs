module Syntax

open System
open Structure

let lex source =
    let rec lex' token str source = seq {
        let emit (token: char list) = seq { if List.length token > 0 then yield token |> List.rev |> String.Concat }
        if str then
            match source with
            | '\\' :: '"' :: t -> yield! lex' ('"' :: token) true t
            | '"' :: t -> 
                yield! emit token
                yield! lex' [] false t
            | c :: t -> yield! lex' (c :: token) true t
            | [] -> failwith "Incomplete string"
        else
            match source with
            | c :: t when Char.IsWhiteSpace c ->
                yield! emit token
                yield! lex' [] false t
            | ('[' as c) :: t | (']' as c) :: t | ('{' as c) :: t | ('}' as c) :: t ->
                yield! emit token
                yield c.ToString()
                yield! lex' [] false t
            | '"' :: t -> yield! lex' ['\''] true t // prefix token with '
            | c :: t -> yield! lex' (c :: token) false t
            | [] -> yield! emit token }
    source |> List.ofSeq |> lex' [] false

type Node =
    | Token of string
    | Quote of Node list
    | Pairs of (string * Node) list

let stripLeadingTick s = if String.length s > 1 then s.Substring(1) else ""

let parse tokens =
    let rec parse' nodes tokens =
        match tokens with
        | "[" :: t ->
            let q, t' = parse' [] t
            parse' (Quote q :: nodes) t'
        | "]" :: t -> List.rev nodes, t
        | "{" :: t ->
            let m, t' = parse' [] t
            let rec pairs list = seq {
                match list with
                | Token n :: v :: t -> yield (if n.StartsWith '\'' then stripLeadingTick n else n), v; yield! pairs t
                | [] -> ()
                | _ -> failwith "Expected name/value pair" }
            parse' (Pairs (m |> pairs |> List.ofSeq) :: nodes) t'
        | "}" :: t -> List.rev nodes, t
        | [] -> List.rev nodes, []
        | token :: t -> parse' (Token token :: nodes) t
    match tokens |> List.ofSeq |> parse' [] with
    | (result, []) -> result
    | _ -> failwith "Unexpected quotation close"

let rec compile nodes =
    let rec compile' node =
        match node with
        | Token t ->
            match Double.TryParse t with
            | (true, v) -> Number v
            | _ ->
                match Boolean.TryParse t with
                | (true, v) -> Boolean v
                | _ -> if t.StartsWith '\'' then (stripLeadingTick t) |> String else Symbol t
        | Quote q -> List (compile q |> List.ofSeq)
        | Pairs p -> p |> Seq.map (fun (n, v) -> n, compile' v) |> Map.ofSeq |> Map
    nodes |> Seq.map compile'

let brief source = source |> lex |> parse |> compile |> List.ofSeq
