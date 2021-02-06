module View.Actions

open Entities.State
open View.TextConstants
open View.Scenes.Index

/// Actions are the bridge between the game core logic and the rendering layer.
/// Each action represents something to be rendered with all the information
/// to do so, without caring how it is processed.
type Action =
  | Message of TextConstant
  | Prompt of Prompt
  | Scene of Scene
  | Effect of (State -> State)
  | NoOp

/// Sequence of actions to be executed.
and ActionChain = Action seq

/// Indicates the need to prompt the user for information.
and Prompt =
  { Title: TextConstant
    Content: PromptContent }

/// Specified the different types of prompts available.
and PromptContent =
  | ChoicePrompt of ChoicePrompt * PromptHandler<Choice>
  | ConfirmationPrompt of PromptHandler<bool>
  | NumberPrompt of PromptHandler<int>
  | TextPrompt of PromptHandler<string>

/// Defines a handler that takes whatever result the prompt is giving out and
/// returns another chain of actions.
and PromptHandler<'a> = 'a -> ActionChain

/// Defines a list of choices that the user can select.
and ChoicePrompt = Choice list

and Choice = { Id: string; Text: TextConstant }

/// Returns a possible choice from a set of choices given its ID.
let choiceById id = List.find (fun c -> c.Id = id)
