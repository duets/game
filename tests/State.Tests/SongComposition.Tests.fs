module State.Tests.SongComposition

open FsUnit
open NUnit.Framework
open Test.Common

open Entities
open Simulation.Queries

[<SetUp>]
let Setup () = Common.initState ()

let unfinishedSong =
    UnfinishedSong(dummySong), 35<quality>, 7<quality>

let startSong () =
    State.Root.apply
    <| SongStarted(dummyBand, unfinishedSong)

[<Test>]
let SongStartedShouldAddUnfinishedSong () =
    startSong ()

    State.Root.get ()
    |> lastUnfinishedSong dummyBand
    |> should equal unfinishedSong

[<Test>]
let SongImprovedShouldReplaceUnfinishedSong () =
    startSong ()

    let improvedSong =
        (UnfinishedSong dummySong, 35<quality>, 14<quality>)

    State.Root.apply
    <| SongImproved(dummyBand, Diff(unfinishedSong, improvedSong))

    State.Root.get ()
    |> lastUnfinishedSong dummyBand
    |> should equal improvedSong

[<Test>]
let SongDiscardedShouldRemoveUnfinishedSong () =
    startSong ()

    State.Root.apply
    <| SongDiscarded(dummyBand, unfinishedSong)

    Songs.unfinishedByBand (State.Root.get ()) dummyBand.Id
    |> should haveCount 0

[<Test>]
let SongFinishedShouldRemoveUnfinishedAndAddFinishedSong () =
    startSong ()

    let finishedSong = (FinishedSong dummySong, 14<quality>)

    State.Root.apply
    <| SongFinished(dummyBand, finishedSong)

    let state = State.Root.get ()

    Songs.unfinishedByBand state dummyBand.Id
    |> should haveCount 0

    state
    |> lastFinishedSong dummyBand
    |> should equal finishedSong
