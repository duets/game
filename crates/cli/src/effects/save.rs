use storage;
use storage::Error;

use crate::shared::action::CliAction;
use crate::shared::context::Context;
use crate::shared::display;

/// Saves the current game state into the savegame file.
pub fn save(context: &Context) -> Option<CliAction> {
    let save_result = storage::save_game_state(context.game_state.clone());

    match save_result {
        Err(error) => match error {
            Error::ContentParsingError => display::show_error(&String::from(
                "It seems like your game state is corrupted. Please try restarting the game",
            )),
            Error::FileWritingError => display::show_error(&String::from(
                "Unable to write the savegame file, please try again",
            )),
            _ => {}
        },
        _ => display::show_info(&String::from("Game saved correctly")),
    }

    None
}
