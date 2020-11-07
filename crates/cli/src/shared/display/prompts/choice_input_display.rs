use crate::shared::action::{Choice, CliAction, PromptText};
use crate::shared::context::Context;
use crate::shared::display;
use crate::shared::input;

/// Handles the display of a choice input, showing the screen's text first, then
/// the different choices available and getting the input of an user making sure
/// that it's inside of the possible choices.
pub fn handle(
    text: PromptText,
    choices: Vec<Choice>,
    on_action: Box<dyn FnOnce(&Choice, &Context) -> CliAction>,
    context: &Context,
) -> CliAction {
    let input = show_choice_input_action(text, &choices);
    on_action(input, context)
}

fn show_choice_input_action(text: PromptText, choices: &[Choice]) -> &Choice {
    display::show_prompt_text_with_new_line(text);
    input::read_choice(choices)
}
