open Renderer

[<EntryPoint>]
let main argv =
    Savegame.load ()
    |> Startup.fromSavegame
    |> fun (state, action) -> Orchestrator.runWith render state (Some action)
    |> ignore
    0
