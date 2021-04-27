module Cli.View.Scenes.RehearsalRoom.Compose

open Cli.View.Actions
open Cli.View.TextConstants
open Cli.View.Scenes.RehearsalRoom.ComposeSong
open Cli.View.Scenes.RehearsalRoom.ImproveSong
open Cli.View.Scenes.RehearsalRoom.FinishSong

let composeOptions =
  [ { Id = "compose_song"
      Text = TextConstant ComposeSong }
    { Id = "improve_song"
      Text = TextConstant ImproveSong }
    { Id = "finish_song"
      Text = TextConstant FinishSong }
    { Id = "discard_song"
      Text = TextConstant DiscardSong }
    { Id = "practice_song"
      Text = TextConstant PracticeSong } ]

let rec compose () =
  seq {
    yield
      Prompt
        { Title = TextConstant ComposePrompt
          Content = ChoicePrompt(composeOptions, processSelection) }
  }

and processSelection choice =
  seq {
    match choice.Id with
    | "compose_song" -> yield! composeSongScene ()
    | "improve_song" -> yield! improveSongScene ()
    | "finish_song" -> yield! finishSongScene ()
    | "discard_song" -> NoOp
    | "practice_song" -> NoOp
    | _ -> NoOp
  }
