import React, { FunctionComponent, useEffect, useState } from 'react'
import Layout from '@ui/components/layout/layout'
import FullSizeSidebar from '@ui/components/sidebars/full-size.sidebar'
import { NavButton } from '@ui/components/buttons/nav/navButton'
import Button from '@ui/components/buttons/button'
import { useSelector } from 'react-redux'
import { State } from '@persistence/store/store'
import { MAX_ASSIGNABLE_LEVEL_POINTS } from '@engine/operations/skill.operations'
import { useCommands } from '@ui/hooks/injections.hooks'
import { useHistory, useLocation } from 'react-router-dom'
import { stringToDate } from '@core/utils/mappers'
import CharacterForm, { CharacterFormInput } from '@ui/screens/character-creation/character.form'
import SkillsForm, { SkillsFormInput } from '@ui/screens/character-creation/skills.form'
import { Gender } from '@engine/entities/gender'
import { useForm } from '@ui/hooks/form.hooks'
import '@ui/styles/screens/character-creation.scss'

const CharacterCreation: FunctionComponent = () => {
    const history = useHistory()
    const startDateParam = useLocation().state
    const gameStartDate = stringToDate(startDateParam)

    const database = useSelector((state: State) => state.database)
    const cities = database.cities
    const instruments = database.instruments

    const [characterInput, updateCharacterInput] = useState<CharacterFormInput>({
        name: '',
        gender: Gender.Male,
        birthday: new Date(),
        originCity: cities[0],
    })
    const [skillsInput, updateSkillInput] = useState<SkillsFormInput>({
        instrument: instruments[0],
    })

    const form = useForm()

    const [assignedPoints, setAssignedPoints] = useState(0)
    const pointsLeft = MAX_ASSIGNABLE_LEVEL_POINTS - assignedPoints
    const characterSkills = useSelector((state: State) => state.gameplay.character.skills)
    useEffect(() => {
        const assigned = characterSkills.reduce((prev, curr) => prev + curr.level, 0)
        setAssignedPoints(assigned)
    }, [characterSkills])

    const { createGame } = useCommands().forms.creation
    const handleGoOn = () => {
        form.clear()

        const result = createGame({
            ...characterInput,
            ...skillsInput,
            gameStartDate,
        })

        form.markValidationErrors(result.errors())
    }

    return (
        <Layout
            className="character-creation"
            left={
                <FullSizeSidebar
                    className="main-menu"
                    navButton={NavButton.Back}
                    onNavButtonClick={history.goBack}
                    header={
                        <CharacterForm
                            form={form}
                            cities={cities}
                            input={characterInput}
                            onUpdate={updateCharacterInput}
                        />
                    }
                />
            }
            right={
                <div className="instruments-skills">
                    <SkillsForm
                        form={form}
                        instruments={instruments}
                        pointsLeft={pointsLeft}
                        input={skillsInput}
                        onUpdate={updateSkillInput}
                    />
                    <Button className="go-button" onClick={handleGoOn}>
                        Go on
                    </Button>
                </div>
            }
        />
    )
}

export default CharacterCreation
