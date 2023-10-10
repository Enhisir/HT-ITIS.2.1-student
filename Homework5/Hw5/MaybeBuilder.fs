module Hw5.MaybeBuilder

type MaybeBuilder() =
    member builder.Bind(a, f): Result<'e,'d> =
        match a with
        | Ok b -> f b
        | Error e -> Error e
    member builder.Return x: Result<'a,'b> = Ok x
let maybe = MaybeBuilder()