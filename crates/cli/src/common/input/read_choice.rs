use dialoguer::{theme::ColorfulTheme, Select};

use crate::common::action::Choice;

/// Given a set of choices shows a menu for the user to select from one of them.
/// Returns the selected choice.
pub fn read_choice(choices: &Vec<Choice>) -> &Choice {
    let choices_name = choices
        .into_iter()
        .map(|c| &c.text)
        .collect::<Vec<&String>>();

    let selection = Select::with_theme(&ColorfulTheme::default())
        .default(0)
        .items(&choices_name)
        .clear(false)
        .interact()
        .unwrap();

    choices.get(selection).unwrap()
}
