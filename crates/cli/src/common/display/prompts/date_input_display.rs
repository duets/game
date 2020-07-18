use chrono::NaiveDate;

use super::common;
use crate::common::action::CliAction;
use crate::common::input;

/// Shows the initial text of the screen, takes the user input as a string and
/// calls the given on_action with the provided input.
pub fn handle(text: &String, on_action: &fn(&NaiveDate) -> CliAction) -> CliAction {
    let input = show_date_input_action(text);
    on_action(&input)
}

fn show_date_input_action(text: &String) -> NaiveDate {
    common::show_start_text(text);
    print!(" Format: dd-mm-YYYY");
    println!();

    get_date()
}

fn get_date() -> NaiveDate {
    let choice_or_error = input::read_date();
    match choice_or_error {
        Ok(choice) => choice,
        Err(_) => get_date_with_error(),
    }
}

fn get_date_with_error() -> NaiveDate {
    println!("Did not recognize a valid date, remember the format is dd-mm-YYYY. Try again:");
    get_date()
}
