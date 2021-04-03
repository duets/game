module Renderer

open View.Actions
open Spectre.Console
open Text

/// Writes a message into the buffer.
let renderMessage message = AnsiConsole.MarkupLine(toString message)

/// Renders the specified prompt and asks the user for a response depending
/// on the specified type of prompt. Returns a string which either represents
/// the raw user input (in case of a TextPrompt) or the ID of the choice that
/// the user chose (in case of a ChoicePrompt).
let renderPrompt prompt =
  match prompt.Content with
  | ChoicePrompt (choices, _) ->
      let mutable selectionPrompt = SelectionPrompt<Choice>()
      selectionPrompt.Title <- toString prompt.Title
      selectionPrompt <- selectionPrompt.AddChoices(choices)

      selectionPrompt <- selectionPrompt.UseConverter(fun c -> toString c.Text)

      AnsiConsole.Prompt(selectionPrompt).Id
  | ConfirmationPrompt _ ->
      AnsiConsole.Confirm(toString prompt.Title)
      |> string
  | NumberPrompt _ ->
      AnsiConsole.Ask<int>(toString prompt.Title)
      |> string
  | TextPrompt _ -> AnsiConsole.Ask<string>(toString prompt.Title)

/// Clears the terminal console.
let clear () = System.Console.Clear()
