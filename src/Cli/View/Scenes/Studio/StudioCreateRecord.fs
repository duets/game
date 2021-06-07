module Cli.View.Scenes.Studio.CreateRecord

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open FSharp.Data.UnitSystems.SI.UnitNames
open Simulation.Queries
open Simulation.Studio.RecordAlbum

let rec createRecordSubscene state studio =
    let currentBand = Bands.currentBand state

    let songOptions =
        finishedSongsSelectorOf state currentBand

    seq {
        if songOptions.Length > 0 then
            yield
                Prompt
                    { Title = TextConstant StudioCreateRecordName
                      Content =
                          TextPrompt
                          <| trackListPrompt
                              state
                              studio
                              currentBand
                              songOptions }
        else
            yield Message <| TextConstant StudioCreateNoSongs
            yield SceneAfterKey Map
    }

and trackListPrompt state studio band songOptions name =
    seq {
        yield
            Prompt
                { Title = TextConstant StudioCreateTrackListPrompt
                  Content =
                      MultiChoicePrompt
                      <| { Choices = songOptions
                           Handler = processRecord state studio band name } }
    }

and processRecord state studio band name selectedSongs =
    let recordingResult =
        finishedSongsFromSelection state band selectedSongs
        |> recordAlbum state studio band name

    seq {
        match recordingResult with
        | Error NameTooShort ->
            yield
                Message
                <| TextConstant StudioCreateErrorNameTooShort

            yield! createRecordSubscene state studio
        | Error NameTooLong ->
            yield
                Message
                <| TextConstant StudioCreateErrorNameTooLong

            yield! createRecordSubscene state studio
        | Error (NotEnoughMoney (bandBalance, studioBill)) ->
            yield
                StudioCreateErrorNotEnoughMoney(bandBalance, studioBill)
                |> TextConstant
                |> Message

            yield SceneAfterKey Map
        | Ok effects -> yield! recordWithProgressBar name effects
        | _ -> yield NoOp
    }

and recordWithProgressBar albumName effects =
    seq {
        yield
            ProgressBar
                { StepNames =
                      [ TextConstant StudioCreateProgressEatingSnacks
                        TextConstant StudioCreateProgressRecordingWeirdSounds
                        TextConstant StudioCreateProgressMovingKnobs ]
                  StepDuration = 3<second>
                  Async = false }

        yield
            Message
            <| TextConstant(StudioCreateAlbumRecorded albumName)

        yield! effects |> Seq.map Effect

        yield SceneAfterKey Map
    }
