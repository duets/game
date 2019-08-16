import React, { FunctionComponent } from 'react'
import useRouter from 'use-react-router'
import Layout from '@ui/components/layout/layout'
import './character-creation.scss'
import FullSizeSidebar from '@ui/components/sidebars/full-size-sidebar/full-size.sidebar'
import TextInput from '@ui/components/inputs/text.input'
import DateInput from '@ui/components/inputs/date.input'
import GenderInput from '@ui/components/inputs/gender/gender.input'
import { NavButton } from '@ui/components/buttons/nav/navButton'

const CharacterCreation: FunctionComponent = () => {
    const { history } = useRouter()

    return (
        <Layout
            left={
                <FullSizeSidebar
                    className="main-menu"
                    header={
                        <div>
                            <h1>Character creation</h1>
                            <TextInput label="Name" />
                            <DateInput label="Birthday" />
                            <GenderInput label="Gender" />

                            <hr />
                            <DateInput label="Game start year" maxDate={new Date()} />
                        </div>
                    }
                    navButton={NavButton.back}
                    onNavButtonClick={history.goBack}
                />
            }
            right={<></>}
        />
    )
}

export default CharacterCreation
