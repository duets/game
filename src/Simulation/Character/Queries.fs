module Simulation.Character.Queries

open Aether
open Entities
open Storage

/// Returns the character that the player is playing with.
let playableCharacter () =
  State.get ()
  |> Optic.get Lenses.State.character_
