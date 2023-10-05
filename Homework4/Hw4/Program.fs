open System
open Hw4.Calculator
open Hw4.Parser

try
    let parsed = parseCalcArguments (Console.ReadLine().Split ' ')
    printfn $"{calculate parsed.arg1 parsed.operation parsed.arg2}"
with e ->
        printfn $"{e.Message}"