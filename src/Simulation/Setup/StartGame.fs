module Simulation.Setup

open Entities

/// Sets up the initial game state based on the data provided by the user in
/// the setup wizard.
let startGame character (band: Band) =
    { Character = character
      CharacterSkills = [ (character.Id, Map.empty) ] |> Map.ofSeq
      CurrentBandId = band.Id
      Bands = [ (band.Id, band) ] |> Map.ofList
      BandRepertoire = Band.Repertoire.emptyFor band.Id
      BankAccounts =
          [ (Character character.Id, BankAccount.forCharacter character.Id)
            (Band band.Id, BankAccount.forBand band.Id) ]
          |> Map.ofSeq
      Today = Calendar.fromDayMonth 1 1 }
    |> GameCreated
