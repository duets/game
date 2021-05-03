[<RequireQualifiedAccess>]
module Storage.State

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

let staticAgent = StateAgent()

/// Returns the state of the game.
let get = staticAgent.Get

/// Sets the state of the game to a given value.
let set = staticAgent.Set

/// Passes the state of the game to the modify function and sets the state to
/// the return value.
let map modify = get () |> modify |> set
