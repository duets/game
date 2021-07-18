namespace Ui

open Entities
open Savegame

[<AutoOpen>]
module Types =
    /// Defines all the screens that come before the game has been initialized
    /// and therefore we don't have direct access to the game state.
    type PreGameScreen =
        | Start
        | Creator
    /// Defines all the screens inside the game.
    type GameScreen = NoOp

    /// Defines the global state before the user gets past the pre-game screens.
    type PreGameState = {
        Screen: PreGameScreen
        Savegame: SavegameState
    }

    /// Defines the global game state common to all screens in the game once
    /// the user gets past the pre-game screens.
    type GameState = { Screen: GameScreen; State: State }

    type UiState =
        | PreGameState of PreGameState
        | GameState of GameState
