namespace State

module Albums =
    open Aether
    open Entities

    let private applyToUnreleased bandId op =
        let unreleasedAlbumsLens =
            Lenses.FromState.Albums.unreleasedByBand_ bandId

        Optic.map unreleasedAlbumsLens op

    let private applyToReleased bandId op =
        let releasedAlbumsLens =
            Lenses.FromState.Albums.releasedByBand_ bandId

        Optic.map releasedAlbumsLens op

    let addUnreleased (band: Band) unreleasedAlbum =
        let (UnreleasedAlbum album) = unreleasedAlbum
        let addUnreleasedAlbum = Map.add album.Id unreleasedAlbum
        applyToUnreleased band.Id addUnreleasedAlbum

    let addReleased (band: Band) releasedAlbum =
        let album = releasedAlbum.Album
        let addReleasedAlbum = Map.add album.Id releasedAlbum
        applyToReleased band.Id addReleasedAlbum

    let removeUnreleased (band: Band) albumId =
        let removeUnreleasedAlbum = Map.remove albumId

        applyToUnreleased band.Id removeUnreleasedAlbum

    let removeReleased (band: Band) albumId =
        let removeReleasedAlbum = Map.remove albumId

        applyToReleased band.Id removeReleasedAlbum
