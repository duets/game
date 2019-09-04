import { Character } from '@engine/entities/character'
import { CharacterSkill } from '@engine/entities/character-skill'

/**
 * Modifies or adds a given skill to a character's set of skills.
 */
export const updateCharacterSkill = (character: Character, skill: CharacterSkill) => {
    const skills = character.skills
    const existingSkill = skills.find(s => s.name === skill.name)

    return {
        ...character,
        skills: [
            ...(existingSkill //
                ? skills.map(s => (s.name === skill.name ? skill : s))
                : [...skills, skill]),
        ],
    }
}
