module Hw4.Parser

open System
open Hw4.Calculator


type CalcOptions = {
    arg1: float
    arg2: float
    operation: CalculatorOperation
}

let isArgLengthSupported (args : string[]) = Array.length args = 3

let parseOperation (arg : string) =
    match arg with
    | "+" -> CalculatorOperation.Plus
    | "-" -> CalculatorOperation.Minus
    | "*" -> CalculatorOperation.Multiply
    | "/" -> CalculatorOperation.Divide
    | _ -> CalculatorOperation.Undefined

let parseDouble (arg: string) =
    let mutable result = 0.0
    match Double.TryParse(arg, &result) with
    | true -> result
    | false -> ArgumentException() |> raise

type parserResult (arg1, operation, arg2) =
    member this.arg1 = arg1
    member this.operation = operation
    member this.arg2 = arg2
    
let parseCalcArguments(args : string[]) =
    match isArgLengthSupported args with
    | true -> (match parseOperation args[1] with
              | CalculatorOperation.Undefined -> InvalidOperationException() |> raise
              | operation -> parserResult(parseDouble args[0], operation, parseDouble args[2]))
    | false ->  ArgumentException() |> raise