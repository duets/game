use super::new_game;
use crate::common::action::Choice;
use crate::common::action::CliAction;
use crate::common::action::Prompt;
use crate::common::screen::Screen;
use crate::effects;

pub fn create_main_screen() -> Screen {
    Screen {
        name: String::from("Main Menu"),
        action: Prompt::ChoiceInput {
            text: String::from("Welcome to Duets! Select an option to begin"),
            choices: vec![
                Choice {
                    id: 0,
                    text: String::from("Start new game"),
                },
                Choice {
                    id: 1,
                    text: String::from("Load game"),
                },
                Choice {
                    id: 2,
                    text: String::from("Exit"),
                },
            ],
            on_action: Box::new(|choice, global_context| match choice.id {
                0 => CliAction::Screen(new_game::create_new_game_screen(global_context)),
                1 => CliAction::SideEffect(effects::exit),
                2 => CliAction::SideEffect(effects::exit),
                _ => CliAction::NoOp,
            }),
        },
    }
}
