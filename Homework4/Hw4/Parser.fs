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
    
let parseCalcArguments(args : string[]) =
    match isArgLengthSupported args with
    | true -> match parseOperation args[1] with
              | CalculatorOperation.Undefined -> ArgumentException() |> raise
              | operation -> {
                  arg1 = parseDouble args[0]
                  arg2 = parseDouble args[2]
                  operation = operation
                  }
    | false -> ArgumentException() |> raise