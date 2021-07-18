namespace Ui

open Elmish
open Avalonia
open Avalonia.Media
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

    type UiState =
        | StartScreenState of Screens.StartScreen.State
        | GameState of State

    let init () =
        StartScreenState { Savegame = NotAvailable }, Cmd.none

    let update msg state =
        match msg with
        | StartScreenMsg startScreenMsg ->
            match state with
            | StartScreenState startScreenState ->
                let (updatedState, cmd) =
                    Screens.StartScreen.update startScreenMsg startScreenState

                match updatedState.Savegame with
                | Available state -> (GameState state, cmd)
                | Incompatible -> (StartScreenState updatedState, cmd)
                | NotAvailable -> (StartScreenState updatedState, cmd)
            | _ -> (state, Cmd.none)
        | Effect effect ->
            match state with
            | StartScreenState _ -> (state, Cmd.none)
            | GameState state ->
                (GameState <| State.Root.apply effect state, Cmd.none)

    let view state dispatch =
        StackPanel.create [
            StackPanel.margin (30.0, 30.0)
            StackPanel.children [
                match state with
                | StartScreenState state ->
                    Screens.StartScreen.view state (StartScreenMsg >> dispatch)
                | GameState _ ->
                    TextBlock.create [
                        TextBlock.text "Coming soon"
                        TextBlock.fontSize 48.0
                    ]
            ]
        ]

    /// Defines the main window of the application which directly interacts
    /// with Avalonia. We set the required width and height as well as different
    /// styles for the app itself.
    type MainWindow() as this =
        inherit HostWindow()

        do
            base.Title <- "Duets"
            base.Width <- 1280.0
            base.Height <- 720.0
            base.MinWidth <- 1280.0
            base.MinHeight <- 720.0
            base.TransparencyLevelHint <- WindowTransparencyLevel.AcrylicBlur
            base.Background <- Brushes.Transparent
            base.ExtendClientAreaToDecorationsHint <- true
#if DEBUG
            base.AttachDevTools()
#endif

            Elmish.Program.mkProgram init update view
            |> Program.withHost this
            |> Program.run
