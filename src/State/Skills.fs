namespace State

module Skills =
    open Aether
    open Aether.Operators
    open Common
    open Entities

    let add (character: Character) (skillWithLevel: SkillWithLevel) =
        let (skill, _) = skillWithLevel

        let skillLens =
            Lenses.State.characterSkills_
            >-> Map.keyWithDefault_ character.Id Map.empty

        let addSkill map = Map.add skill.Id skillWithLevel map

        Optic.map skillLens addSkill
