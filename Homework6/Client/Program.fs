open System.Net.Http

let isArgLengthSupported (args:string[]): Result<string[], string>=
    if args.Length = 3 then Ok args else Error "Недостаточно аргументов"

[<EntryPoint>]
let main (args: string[]) =
    match args |> isArgLengthSupported with
    | Ok args ->
            async {
                try
                    let url = "http://localhost:5000/calculate?"
                              + $"value1={args[0]}&operation={args[1]}&value2={args[2]}"
                    
                    use client = new HttpClient()
                    let! response = client.GetStringAsync(url) |> Async.AwaitTask
                    printfn $"{response}"
                with
                | ex -> printfn $"{ex.Message}";
            } |> Async.RunSynchronously
    | Error e -> printfn $"{e}"
    0