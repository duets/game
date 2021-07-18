namespace Ui.Screens

open System
open Avalonia.Controls
open Avalonia.Layout
open Elmish
open Avalonia.FuncUI.DSL

open Savegame

module StartScreen =
    /// Current version of the game as loaded from the fsproj.
    let version =
        System
            .Reflection
            .Assembly
            .GetEntryAssembly()
            .GetName()
            .Version.ToString()

    type State = { Savegame: SavegameState }

    type Msg =
        | SetSavegame of SavegameState
        | Exit

    let init () = { Savegame = NotAvailable }, Cmd.none

    let update msg state =
        match msg with
        | SetSavegame savegame -> ({ state with Savegame = savegame }, Cmd.none)
        | Exit ->
            Environment.Exit(0)
            (state, Cmd.none)

    let private button attrs =
        Button.create [
            yield! attrs
            Button.margin (0.0, 5.0)
            Button.maxWidth 200.0
            Button.horizontalAlignment HorizontalAlignment.Stretch
            Button.horizontalContentAlignment HorizontalAlignment.Center
        ]

    let private isSavegameAvailable state =
        match state.Savegame with
        | Available _ -> true
        | _ -> false

    let view (state: State) dispatch =
        DockPanel.create [
            DockPanel.horizontalAlignment HorizontalAlignment.Stretch
            DockPanel.verticalAlignment VerticalAlignment.Stretch
            DockPanel.children [
                StackPanel.create [
                    StackPanel.dock Dock.Top
                    StackPanel.children [
                        TextBlock.create [
                            TextBlock.horizontalAlignment
                                HorizontalAlignment.Center
                            TextBlock.fontSize 48.0
                            TextBlock.text "Duets"
                        ]

                        TextBlock.create [
                            TextBlock.horizontalAlignment
                                HorizontalAlignment.Center
                            TextBlock.fontSize 12.0
                            TextBlock.text $"v{version}"
                        ]
                    ]
                ]

                StackPanel.create [
                    StackPanel.children [
                        button [ Button.content "New game" ]

                        button [
                            Button.content "Load game"
                            Button.isEnabled (isSavegameAvailable state)
                        ]

                        button [
                            Button.content "Exit"
                            Button.onClick (fun _ -> dispatch Exit)
                        ]
                    ]
                ]
            ]
        ]
