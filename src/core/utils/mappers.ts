import { fromPredicate, none, Option, some } from 'fp-ts/lib/Option'
import { findFirst } from 'fp-ts/lib/Array'
import { Gender } from '@engine/entities/gender'
import { City } from '@engine/entities/city'
import { Instrument } from '@engine/entities/instrument'

export const stringToString = (value: string) => value

const getOptionFromDate = fromPredicate((date: Date) => !isNaN(date.getDate()))
export const stringToMaybeDate = (value: string): Option<Date> => getOptionFromDate(new Date(value))

export const stringToMaybeGender = (value: string): Option<Gender> => {
    switch (value) {
        case 'male':
            return some(Gender.male)
        case 'female':
            return some(Gender.female)
        default:
            return none
    }
}

export const stringToMaybeCity = (value: string, cities: ReadonlyArray<City>) =>
    findFirst((city: City) => city.name === value)([...cities])

export const stringToMaybeInstrument = (value: string, instruments: ReadonlyArray<Instrument>) =>
    findFirst((instrument: Instrument) => instrument.name === value)([...instruments])
