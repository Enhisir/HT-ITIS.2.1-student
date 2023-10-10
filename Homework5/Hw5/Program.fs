open Hw5

let main (args: string[]) =
    match Parser.parseCalcArguments args with
    | Ok (arg1, operation, arg2) ->
        printfn "Результат: %f" (Calculator.calculate arg1 operation arg2)
    | Error errorValue ->
        printfn "Прервано: %s"(match errorValue with
                               | Message.DivideByZero -> "деление на ноль невозможно"
                               | Message.WrongArgFormat -> "агрументы не соответствуют ожидаемым"
                               | Message.WrongArgLength -> "количество аргументов не соответствуют ожидаемому"
                               | Message.WrongArgFormatOperation -> "операция не распознана"
                               | _ -> "возникла непредвиденная ошибка")