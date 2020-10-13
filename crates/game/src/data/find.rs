use common::entities::Identity;

/// Finds an entity in a given list by its ID.
pub fn find_in<'a, I>(list: &'a [I], entity: &I) -> Option<&'a I>
where
    I: Identity + Sized,
{
    list.iter().find(|element| element.id() == entity.id())
}
