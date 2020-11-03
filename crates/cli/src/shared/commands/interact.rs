use std::rc::Rc;
use std::sync::Arc;

use common::entities::Object;
use game::context::Context;
use game::world::interactions;
use game::world::interactions::{InteractItem, Interaction, Requirement};

use super::Command;
use crate::screens::GameScreen;
use crate::shared::action::{Choice, CliAction, Prompt, PromptText};
use crate::shared::display;
use crate::shared::lang;
use crate::shared::parsers;

/// Allows the user to get a list of all the objects available in the current room.
pub fn create_interact_command() -> Command {
    Command {
        name: String::from("interact"),
        matching_names: vec![String::from("i")],
        explanation: String::from(
            "Shows the available interactions with a given object and executes one",
        ),
        help: r#"
interact
--------
Shows the available interactions with a given object and executes one. Should be invoked with at
least one parameters which is the name of the object to interact with.
        "#
        .into(),
        execute: Arc::new(move |args, global_context| {
            if !args.is_empty() {
                let object = parsers::parse_object_from(&args, global_context);
                match object {
                    Some(object) => show_interactions(object),
                    _ => {
                        display::show_error(&format!(
                            "No object found with the name {}",
                            lang::transformations::join_vec(&args)
                        ));
                        CliAction::Continue
                    }
                }
            } else {
                display::show_error("No object given. What do you want to interact with?");
                CliAction::Continue
            }
        }),
    }
}

fn show_interactions(object: Object) -> CliAction {
    let object_interactions = interactions::get_for(&object);

    CliAction::Prompt(Prompt::ChoiceInput {
        text: PromptText::WithEmoji(format!("What do you want to do with the {}?", object.name)),
        choices: object_interactions
            .iter()
            .enumerate()
            .map(|(index, interaction)| Choice {
                id: index,
                text: format!("{} - {}", interaction.name(), interaction.description()),
            })
            .collect(),
        on_action: Box::new(move |choice, global_context| {
            let chosen_interaction = object_interactions.into_iter().nth(choice.id).unwrap();
            let interaction_result = interactions::sequence(&*chosen_interaction, &global_context);

            match interaction_result {
                Ok(sequence) => show_sequence(chosen_interaction, sequence, &global_context),
                Err(requirement) => match requirement {
                    Requirement::HealthAbove(_) => {
                        display::show_error("You don't have enough health to do that");
                        CliAction::Continue
                    }
                    Requirement::MoodAbove(_) => {
                        display::show_error("You don't have enough mood to do that");
                        CliAction::Continue
                    }
                    Requirement::EnergyAbove(_) => {
                        display::show_error("You don't have enough energy to do that");
                        CliAction::Continue
                    }
                },
            }
        }),
    })
}

fn show_sequence(
    interaction: Rc<dyn Interaction>,
    sequence: InteractItem,
    context: &Context,
) -> CliAction {
    CliAction::Screen(GameScreen::Interaction {
        interaction,
        sequence,
        context: context.clone(),
    })
}
