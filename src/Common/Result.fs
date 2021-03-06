module Common.Result

open System

/// Returns another result if the current one evaluates to Ok, otherwise
/// returns the error.
let andThen chainedResult = Result.bind (fun _ -> chainedResult)

/// Returns the given value if the result is Ok, otherwise returns the error.
let transform value = Result.bind (fun _ -> Ok value)

/// Returns the OK side of a result or raises an exception otherwise. Only use
/// when the result is KNOWN to only have an OK.
let unwrap result =
    match result with
    | Ok res -> res
    | Error _ -> raise (NullReferenceException())

/// Returns the Error side of a result or raises an exception otherwise.
let unwrapError result =
    match result with
    | Ok _ -> raise (NullReferenceException())
    | Error error -> error
