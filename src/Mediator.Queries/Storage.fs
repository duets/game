module Mediator.Queries.Storage

open Mediator.Queries.Types

type SavegameState = NotAvailable | Available

/// Retrieves whether there's a savegame available for loading or not.
let SavegameStateQuery : Query<unit, SavegameState> = {
  Id = QueryId.SavegameState
  Parameter = None
  ResultType = typeof<SavegameState>
}

type Role = string
type Genre = string

/// Retrieves the static list of roles available in the game.
let RolesQuery : Query<unit, Role list> = {
  Id = QueryId.Roles
  Parameter = None
  ResultType = typeof<Role list>
}

/// Retrieves the static list of genres available in the game.
let GenresQuery : Query<unit, Genre list> = {
  Id = QueryId.Genres
  Parameter = None
  ResultType = typeof<Genre list>
}