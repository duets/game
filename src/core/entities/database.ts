import { City } from '@engine/entities/city'
import { Instrument } from '@engine/entities/instrument'
import { Skill } from '@engine/entities/skill'
import { Genre } from '@engine/entities/genre'
import { Role } from '@engine/entities/role'

/**
 * Defines the information saved in the static database that is downloaded from the game.
 */
export type Database = {
    instruments: ReadonlyArray<Instrument>
    cities: ReadonlyArray<City>
    skills: ReadonlyArray<Skill>
    genres: ReadonlyArray<Genre>
    roles: ReadonlyArray<Role>
}
