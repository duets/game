module Savegame

open Common
open Entities

type SavegameState =
    | Available of State
    | NotAvailable
    | Incompatible

/// Attempts to read the savegame from the file and sets the state with it,
/// returning whether it was available or not.
let private loadStateFromSavegame () =
    Files.savegamePath ()
    |> Files.readAll
    |> Option.bind Serializer.deserialize
    |> Option.map Available
    |> Option.defaultValue NotAvailable

/// Attempts to write the given state into the savegame file.
let private writeSavegame (state: State) =
    state
    |> Serializer.serialize
    |> Files.write (Files.savegamePath ())

type SavegameAgentMessage =
    | Read of AsyncReplyChannel<SavegameState>
    | Write of State

/// Agent in charge of writing and loading the savegame from a file.
/// The reason behind having these operations in an agent is that, since we need
/// to execute the saving constantly, doing it asynchronously is a must to not
/// block the UI and an agent provides this plus the safety of knowing that all
/// these operations will still be done in just one thread which means that the
/// saving will never be done while reading, no multiple writes, etc.
type private SavegameAgent() =
    let agent =
        MailboxProcessor.Start
        <| fun inbox ->
            let rec loop () =
                async {
                    let! msg = inbox.Receive()

                    match msg with
                    | Read channel ->
                        try
                            loadStateFromSavegame () |> channel.Reply
                        with _ -> channel.Reply Incompatible
                    | Write state -> writeSavegame state

                    return! loop ()
                }

            loop ()

    member this.Read() = agent.PostAndReply Read
    member this.Write state = state |> Write |> agent.Post

let private savegameAgent = SavegameAgent()

/// Attempts to load the savegame from a file and returns whether it was present
/// or not. If present, sets the state of the storage to the one loaded from
/// the savegame file.
let load = savegameAgent.Read

/// Attempts to write the current state into the savegame file doing so in a
/// separate thread.
let save = savegameAgent.Write
