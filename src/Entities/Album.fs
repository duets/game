module Entities.Album

open Common
open FSharp.Data.UnitSystems.SI.UnitNames

type CreationError =
    | NameTooShort
    | NameTooLong
    | NoSongsSelected

let private twentyFiveMinutes = 25 * 60<second>

/// Determines the length of the given track list.
let length trackList =
    List.fold
        (fun albumLength ((FinishedSong s), _) ->
            albumLength + Time.Length.inSeconds s.Length)
        0<second>
        trackList

type RecordTypeError = EmptyTrackList

/// Determines the record type of an album given its track list.
let recordType trackList =
    if List.length trackList = 0 then
        Error EmptyTrackList
    else if List.length trackList = 1 then
        Ok Single
    else
        match length trackList with
        | l when l <= twentyFiveMinutes -> Ok EP
        | _ -> Ok LP

let private validateName name =
    match String.length name with
    | l when l < 1 -> Error NameTooShort
    | l when l > 100 -> Error NameTooLong
    | _ -> Ok()

let private validateTrackList trackList =
    match List.isEmpty trackList with
    | true -> Error NoSongsSelected
    | _ -> Ok()

/// Creates an album given its name and the list of songs that define the track
/// list.
let from (name: string) (trackList: RecordedSong list) =
    validateName name
    |> Result.bind (fun _ -> validateTrackList trackList)
    |> Result.bind
        (fun _ ->
            Ok
                { Id = AlbumId <| Identity.create ()
                  Name = name
                  TrackList = trackList
                  // We've already validated the track list before.
                  Type = recordType trackList |> Result.unwrap })

module Unreleased =
    /// Creates an unreleased album given a name and a track list.
    let from name trackList =
        from name trackList |> Result.map UnreleasedAlbum

    /// Modifies the name of the given album validating that it's correct.
    let modifyName (UnreleasedAlbum album) name =
        validateName name
        |> Result.bind
            (fun _ -> Ok <| UnreleasedAlbum { album with Name = name })

module Released =
    /// Updates an already released album with the new amount of streams and
    /// hype.
    let update album streams hype =
        { album with
              Streams = streams
              Hype = hype }

    /// Transforms a given unreleased album into its released status.
    let fromUnreleased
        (UnreleasedAlbum album)
        releaseDate
        maxDailyStreams
        hype
        =
        { Album = album
          ReleaseDate = releaseDate
          Streams = 0
          MaxDailyStreams = maxDailyStreams
          Hype = hype }
