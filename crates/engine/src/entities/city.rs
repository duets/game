/// Defines a city in the game. Must belong to a country.
#[derive(Clone)]
pub struct City {
    pub name: String,
    pub country_name: String,
    pub population: i32,
}
