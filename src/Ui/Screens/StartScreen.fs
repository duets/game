namespace Ui.Screens

open Avalonia.Controls
open Avalonia.Layout
open Avalonia.FuncUI.DSL
open Elmish
open System

open Ui.Types

module StartScreen =
    /// Current version of the game as loaded from the fsproj.
    let version =
        System
            .Reflection
            .Assembly
            .GetEntryAssembly()
            .GetName()
            .Version.ToString()

    type Msg =
        | LoadSavegame
        | SetSavegame of Savegame.SavegameState
        | Exit

    let init () =
        ({ Screen = Start
           Savegame = Savegame.NotAvailable },
         Cmd.ofMsg LoadSavegame)

    let update msg state =
        match msg with
        | LoadSavegame ->
            Savegame.load ()
            |> fun savegame -> (state, Cmd.ofMsg (SetSavegame savegame))
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
        | Savegame.Available _ -> true
        | _ -> false

    let private isIncompatibleSavegame state =
        match state.Savegame with
        | Savegame.Incompatible -> true
        | _ -> false

    let view (state: PreGameState) dispatch =
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
                    StackPanel.margin (0.0, 20.0)
                    StackPanel.children [
                        if isIncompatibleSavegame state then
                            TextBlock.create [
                                TextBlock.horizontalAlignment
                                    HorizontalAlignment.Center
                                TextBlock.margin (0.0, 20.0)
                                TextBlock.classes [ "error" ]
                                TextBlock.text
                                    "Your savegame was incompatible and was ignored. You'll need to manually correct it if you want to use it"
                            ]

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
