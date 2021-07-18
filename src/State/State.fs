[<RequireQualifiedAccess>]
module State.Root

open Entities

/// Applies an effect to the state.
let apply effect state =
    match effect with
    | GameCreated initialState -> initialState
    | TimeAdvanced time -> Calendar.setTime time state
    | SongStarted (band, unfinishedSong) ->
        Songs.addUnfinished band unfinishedSong state
    | SongImproved (band, (Diff (_, unfinishedSong))) ->
        Songs.addUnfinished band unfinishedSong state
    | SongFinished (band, finishedSong) ->
        let song = Song.fromFinished finishedSong

        Songs.removeUnfinished band song.Id state
        |> Songs.addFinished band finishedSong
    | SongDiscarded (band, unfinishedSong) ->
        let song = Song.fromUnfinished unfinishedSong
        Songs.removeUnfinished band song.Id state
    | MemberHired (band, currentMember) ->
        Bands.addMember band currentMember state
    | MemberFired (band, currentMember, pastMember) ->
        Bands.removeMember band currentMember state
        |> Bands.addPastMember band pastMember
    | SkillImproved (character, Diff (_, skill)) ->
        Skills.add character skill state
    | MoneyTransferred (account, transaction) ->
        Bank.transfer account transaction state
    | MoneyEarned (account, transaction) ->
        Bank.transfer account transaction state
    | AlbumRecorded (band, album) ->
        let updatedState = Albums.addUnreleased band album state
        let (UnreleasedAlbum ua) = album

        ua.TrackList
        |> List.map (fun ((FinishedSong fs), _) -> fs.Id)
        |> List.fold
            (fun state song -> Songs.removeFinished band song state)
            updatedState
    | AlbumRenamed (band, unreleasedAlbum) ->
        let (UnreleasedAlbum album) = unreleasedAlbum

        Albums.removeUnreleased band album.Id state
        |> Albums.addUnreleased band unreleasedAlbum
    | AlbumReleased (band, releasedAlbum) ->
        let album = releasedAlbum.Album

        Albums.removeUnreleased band album.Id state
        |> Albums.addReleased band releasedAlbum
    | AlbumReleasedUpdate (band, releasedAlbum) ->
        let album = releasedAlbum.Album

        Albums.removeReleased band album.Id state
        |> Albums.addReleased band releasedAlbum
    | GenreMarketsUpdated genreMarkets -> Market.set genreMarkets state
