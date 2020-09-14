use app::context::Context;
use app::world::interactions::{interact_with, Interaction, Requirement};
use common::entities::Object;

fn get_interaction() -> Interaction {
    Interaction {
        name: String::default(),
        description: String::default(),
        object: Object::default(),
        requirements: vec![],
    }
}

#[test]
fn interact_with_should_fail_if_requirements_not_met() {
    let context = Context::default();
    let interaction = Interaction {
        requirements: vec![Requirement::Health(101)],
        ..get_interaction()
    };
    let interact_result = interact_with(interaction, &context);

    match interact_result {
        Ok(_) => panic!("Not supposed to be okay."),
        Err(err) => assert_eq!(err, "Your health should be at least 101 to do this"),
    }
}
