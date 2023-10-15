module Hw6.App

open Hw5.Calculator
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Hw5

[<CLIMutable>]
type argsQuery =
      {
       value1: double
       operation: string
       value2: double
       }

let parseOperation (op: string) =
    match op with
    | "Plus"     -> Ok CalculatorOperation.Plus
    | "Minus"    -> Ok CalculatorOperation.Minus
    | "Multiply" -> Ok CalculatorOperation.Multiply
    | "Divide"   -> Ok CalculatorOperation.Divide
    | _          -> Error $"Could not parse value '{op}'"
    
let calculate value1 operation value2: Result<string, string> =
    match operation with
    | CalculatorOperation.Plus     -> Ok $"{value1 + value2}"
    | CalculatorOperation.Minus    -> Ok $"{value1 - value2}"
    | CalculatorOperation.Multiply -> Ok $"{value1 * value2}"
    | CalculatorOperation.Divide   -> if value2 <> 0.0 then Ok $"{value1 / value2}" else Ok "DivideByZero"
    | _                            -> Error $"Could not parse value '{operation}'"

let calculatorHandler: HttpHandler =
    fun next ctx ->
        let result: Result<string, string> = MaybeBuilder.maybe {
            let! argsQuery = ctx.TryBindQueryString<argsQuery>()
            let! operation = parseOperation argsQuery.operation
            let! computed = calculate argsQuery.value1 operation argsQuery.value2
            return computed
        }

        match result with
        | Ok ok -> (setStatusCode 200 >=> text (ok.ToString())) next ctx
        | Error error -> (setStatusCode 400 >=> text error) next ctx

let webApp =
    choose [
        GET >=> choose [
             route "/" >=> text "Use //calculate?value1=<VAL1>&operation=<OPERATION>&value2=<VAL2>"
             route "/calculate" >=> calculatorHandler
        ]
        setStatusCode 404 >=> text "Not Found" 
    ]
    
type Startup() =
    member _.ConfigureServices (services : IServiceCollection) =
        services.AddGiraffe() |> ignore

    member _.Configure (app : IApplicationBuilder) (_ : IHostEnvironment) (_ : ILoggerFactory) =
        app.UseGiraffe webApp
        
[<EntryPoint>]
let main _ =
    Host
        .CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun whBuilder -> whBuilder.UseStartup<Startup>() |> ignore)
        .Build()
        .Run()
    0