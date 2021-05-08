﻿module State

open Entities

type StateMessage =
  | Get of AsyncReplyChannel<State>
  | Set of State

type StateAgent() =
  let state =
    MailboxProcessor.Start
    <| fun inbox ->
         let rec loop state =
           async {
             let! msg = inbox.Receive()

             match msg with
             | Get channel ->
                 channel.Reply state
                 return! loop state
             | Set value -> return! loop value
           }

         loop State.empty

  member this.Get() = state.PostAndReply Get

  member this.Set value = Set value |> state.Post

  member this.Map fn = this.Get() |> fn |> this.Set

let staticAgent = StateAgent()

/// Returns the state of the game.
let get = staticAgent.Get

/// Applies an effect to the state.
let apply effect =
  match effect with
  | GameCreated state -> staticAgent.Set state
  | SongStarted (band, unfinishedSong) ->
      State.Songs.addUnfinished staticAgent.Map band unfinishedSong
  | SongImproved (band, (Diff (_, unfinishedSong))) ->
      State.Songs.addUnfinished staticAgent.Map band unfinishedSong
  | SongFinished (band, finishedSong) ->
      let song = Song.fromFinished finishedSong
      State.Songs.removeUnfinished staticAgent.Map band song.Id
      State.Songs.addFinished staticAgent.Map band finishedSong
  | SongDiscarded (band, unfinishedSong) ->
      let song = Song.fromUnfinished unfinishedSong
      State.Songs.removeUnfinished staticAgent.Map band song.Id
