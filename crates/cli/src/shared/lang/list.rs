use common::entities::{Object, Room};
use in_definite;

/// Transforms a list of elements into a description of the list handling the use of a/an, commans
/// and 'and' in the last element. Example:
/// ['guitar', 'fake guitar'] -> "a guitar and a fake guitar"
pub fn describe(elements: &Vec<String>) -> String {
    let mut list_description = String::default();

    for (index, element) in elements.iter().enumerate() {
        let separator = if index == 0 {
            ""
        } else if index == elements.len() - 1 {
            " and "
        } else {
            ", "
        };

        list_description.push_str(&format!(
            "{}{} {}",
            separator,
            in_definite::get_a_or_an(&element),
            element
        ));
    }

    list_description
}

/// Transforms a list of objets into a description of the list using `describe`.
pub fn describe_objects(objects: &Vec<Object>) -> String {
    let objects_description = objects.iter().map(|object| object.name.clone()).collect();
    describe(&objects_description)
}

/// Transforms a list of rooms into a description of the list using `describe`.
pub fn describe_rooms(rooms: &Vec<Room>) -> String {
    let rooms_description = rooms.iter().map(|room| room.name.clone()).collect();
    describe(&rooms_description)
}
