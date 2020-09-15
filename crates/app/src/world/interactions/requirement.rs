use boolinator::Boolinator;

use engine::operations::character;

use super::InteractResult;
use crate::context::Context;

/// Creates an empty ok result for InteractResult.
fn empty_result(context: &Context) -> InteractResult {
    Ok(empty_tuple(context))
}

fn empty_tuple(context: &Context) -> (String, Context) {
    (String::default(), context.clone())
}

/// Defines the different set of requirements that can be possibly added to an interaction.
#[derive(Clone)]
pub enum Requirement {
    Health(i8),
    Mood(i8),
}

/// Checks that all the given requirements are met. Returns an error if something was not met
/// or an empty result otherwise to continue.
pub fn check_requirements(context: &Context, requirements: Vec<Requirement>) -> InteractResult {
    requirements
        .into_iter()
        .map(|req| check_requirement(context, req))
        .fold(empty_result(context), |acc, req_result| {
            acc.and_then(|_| req_result)
        })
}

/// Checks that the given requirement is met.
fn check_requirement(context: &Context, requirement: Requirement) -> InteractResult {
    match requirement {
        Requirement::Health(min_health) => {
            character::health_above(&context.game_state.character, min_health).as_result(
                empty_tuple(context),
                format!("Your health should be at least {} to do this", min_health),
            )
        }
        Requirement::Mood(min_mood) => {
            character::mood_above(&context.game_state.character, min_mood).as_result(
                empty_tuple(context),
                format!("Your mood should be at least {} to do this", min_mood),
            )
        }
    }
}
