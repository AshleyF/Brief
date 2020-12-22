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
            | ('[' as c) :: t | (']' as c) :: t ->
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

let parse tokens =
    let rec parse' nodes tokens =
        match tokens with
        | "[" :: t ->
            let q, t' = parse' [] t
            parse' (Quote q :: nodes) t'
        | "]" :: t -> List.rev nodes, t
        | [] -> List.rev nodes, []
        | token :: t -> parse' (Token token :: nodes) t
    match tokens |> List.ofSeq |> parse' [] with
    | (result, []) -> result
    | _ -> failwith "Unexpected quotation close"

let rec compile nodes = seq {
    match nodes with
    | Token t :: n ->
        match Double.TryParse t with
        | (true, v) -> yield Number v
        | _ ->
            match Boolean.TryParse t with
            | (true, v) -> yield Boolean v
            | _ ->
                if t.StartsWith '\'' then yield (if t.Length > 1 then t.Substring(1) else "") |> String
                else yield Symbol t
        yield! compile n
    | Quote q :: n ->
        yield List (compile q |> List.ofSeq)
        yield! compile n
    | [] -> () }

let brief source = source |> lex |> parse |> compile |> List.ofSeq