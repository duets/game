use common::entities::SkillWithLevel;
use common::shared::bound_to_positive_hundred;

pub trait SkillCommands {
    /// Increases the skill by the given amount only if possible (< 100), otherwise sets it to 100.
    fn increase_by(&self, amount: u8) -> SkillWithLevel;
    /// Decreases the skill by the given amount only if possible (> 100), otherwise sets it to 0.
    fn decrease_by(&self, amount: u8) -> SkillWithLevel;
}

impl SkillCommands for SkillWithLevel {
    fn increase_by(&self, amount: u8) -> SkillWithLevel {
        let mut cloned = self.clone();
        cloned.level = bound_to_positive_hundred(cloned.level.saturating_add(amount));
        cloned
    }

    fn decrease_by(&self, amount: u8) -> SkillWithLevel {
        let mut cloned = self.clone();
        cloned.level = cloned.level.saturating_sub(amount);
        cloned
    }
}
