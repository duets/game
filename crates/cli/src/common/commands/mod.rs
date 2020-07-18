mod help;

use crate::common::screen::Screen;

/// General error for when we couldn't match the given user input with an
/// exiting command.
pub struct InvalidInputError;

/// Defines the common fields that any command should have.
pub struct Command {
    /// Friendly name of the command. Example: help, install.
    pub name: String,

    /// Explanation that will be given when the user asks for help.
    pub explanation: String,

    /// List of matching names that can invoke the command. Example: help, h.
    pub matching_names: Vec<String>,

    /// Function to call when the command is executed. The args passed to the
    /// command will be passed plus a Screen that defines the current context
    /// in which the game is in.
    pub execute: Box<dyn Fn(Vec<String>, Screen)>,
}
