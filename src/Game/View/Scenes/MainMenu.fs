module View.Scenes.MainMenu

open View.Actions
open View.Scenes.Index
open View.TextConstants

let menuOptions =
  [ { Id = "new_game"
      Text = MainMenuNewGame }
    { Id = "load_game"
      Text = MainMenuLoadGame }
    { Id = "exit"; Text = MainMenuExit } ]

/// Creates the main menu of the game as a sequence of actions.
let rec mainMenu () =
  seq {
    yield Message MainMenuTitle

    yield
      Prompt
        { Title = MainMenuPrompt
          Content = ChoicePrompt(menuOptions, processSelection) }
  }

and processSelection choice =
  seq {
    match choice.Id with
    | "new_game" -> yield (Scene CharacterCreator)
    | "load_game" -> yield! []
    | "exit" -> yield NoOp
    | _ -> yield NoOp
  }
