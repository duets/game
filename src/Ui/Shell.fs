namespace Ui

open Elmish
open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI
open Avalonia.FuncUI.Components.Hosts
open Avalonia.FuncUI.Elmish

open Entities
open Savegame

/// This is the main view of the UI in which we handle the upper level views
/// as well as the global UI state.
module Shell =
    type Msg =
        | StartScreenMsg of Screens.StartScreen.Msg
        | Effect of Effect

    let init () =
        (PreGameState
            { Screen = PreGameScreen.Start
              Savegame = NotAvailable },
         Cmd.none)

    let update msg state : UiState * Cmd<_> =
        match msg with
        | StartScreenMsg startScreenMsg ->
            match state with
            | PreGameState preGameState ->
                let (updatedState, cmd) =
                    Screens.StartScreen.update startScreenMsg preGameState

                match updatedState.Savegame with
                // TODO: Change when loading is implemented.
                | Available _ ->
                    (PreGameState preGameState, Cmd.map StartScreenMsg cmd)
                | _ -> (PreGameState updatedState, Cmd.map StartScreenMsg cmd)
            | _ -> (state, Cmd.none)
        | Effect effect ->
            match state with
            | PreGameState _ -> (state, Cmd.none)
            | GameState gameState ->
                (GameState
                    { gameState with
                          State = State.Root.apply effect gameState.State },
                 Cmd.none)

    let view state dispatch =
        StackPanel.create [
            StackPanel.margin (30.0, 30.0)
            StackPanel.children [
                match state with
                | PreGameState state ->
                    Screens.StartScreen.view state (StartScreenMsg >> dispatch)
                | GameState _ ->
                    TextBlock.create [
                        TextBlock.text "Coming soon"
                        TextBlock.fontSize 48.0
                    ]
            ]
        ]

    /// Entry point to the application, which loads Elmish and runs the main
    /// view defined above.
    type MainControl() as this =
        inherit HostControl()

        do
            Elmish.Program.mkProgram init update view
            |> Program.withHost this
            |> Program.run
