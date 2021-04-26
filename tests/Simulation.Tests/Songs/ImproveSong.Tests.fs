module Simulation.Tests.ImproveSong

open Test.Common
open NUnit.Framework
open FsUnit

open Entities
open Simulation.Songs.Composition.ComposeSong
open Simulation.Songs.Composition.ImproveSong

[<SetUp>]
let Setup () =
  initStateWithDummies ()
  let character = currentCharacter ()
  addSkillTo character (Skill.createWithLevel SkillId.Composition 50)
  addSkillTo character (Skill.createWithLevel (Genre dummyBand.Genre) 50)
  composeSong dummySong

[<Test>]
let ShouldImproveIfPossibleAndReturnCanBeImproved () =
  let song = lastUnfinishedSong ()

  improveSong song
  |> should be (ofCase <@ CanBeImproved 14<quality> @>)

[<Test>]
let ShouldImproveForALastTimeIfPossibleAndReturnReachedMaxQuality () =
  improveLastUnfinishedSongTimes 4

  lastUnfinishedSong ()
  |> improveSong
  |> should be (ofCase <@ ReachedMaxQuality 33<quality> @>)
