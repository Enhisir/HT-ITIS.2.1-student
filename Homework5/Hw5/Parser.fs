﻿module Hw5.Parser

open System
open System.Globalization
open Hw5.Calculator
open Hw5.MaybeBuilder

let isArgLengthSupported (args:string[]): Result<string[], Message>=
    if args.Length = 3 then Ok args else Error Message.WrongArgLength

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isOperationSupported (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), Message> =
    match operation with
    | Calculator.plus -> Ok (arg1, CalculatorOperation.Plus, arg2)
    | Calculator.minus -> Ok (arg1, CalculatorOperation.Minus, arg2)
    | Calculator.multiply -> Ok (arg1, CalculatorOperation.Multiply, arg2)
    | Calculator.divide -> Ok (arg1, CalculatorOperation.Divide, arg2)
    | _ -> Error Message.WrongArgFormatOperation
    
let parseDouble (arg : string) =
    let culture = CultureInfo("en-US")
    match Double.TryParse(arg, NumberStyles.Any, culture) with
    | true, value -> Ok value
    | _ -> Error Message.WrongArgFormat

let parseArgs (args: string[]): Result<float * CalculatorOperation * float, Message> =
    maybe{
        let! first = parseDouble args[0]
        let! second = parseDouble args[2]
        let! result = isOperationSupported(first, args[1], second)
        return result
    }

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isDividingByZero (arg1, operation, arg2): Result<float * CalculatorOperation * float, Message> =
    if operation = CalculatorOperation.Divide && arg2.ToString() = "0"
    then Error Message.DivideByZero
    else Ok (arg1, operation, arg2)   
    
let parseCalcArguments (args: string[]): Result<float * CalculatorOperation * float, Message> =
    maybe{
        let! args = isArgLengthSupported args
        let! args = parseArgs args
        let! args = isDividingByZero args
        return args
    }