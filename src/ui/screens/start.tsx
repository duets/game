import React, { FunctionComponent, useContext } from 'react'
import { pipe } from 'fp-ts/lib/pipeable'
import { fold } from 'fp-ts/lib/TaskEither'
import { of } from 'fp-ts/lib/Task'
import { useSelector } from 'react-redux'
import FullSizeSidebar from '@ui/components/sidebars/full-size.sidebar'
import Layout from '@ui/components/layout/layout'
import PlayButton from '@ui/components/buttons/play.button'
import Changelog from '@ui/components/changelog/changelog'
import { GameInfoContext } from '@ui/contexts/game-info.context'
import { useActions } from '@ui/hooks/injections.hooks'
import { useMountEffect } from '@ui/hooks/mount.hooks'
import { State } from '@persistence/store/store'
import { ChangelogsState } from '@persistence/store/changelogs/changelogs.state'
import { NavButton } from '@ui/components/buttons/nav/navButton'
import '@ui/styles/screens/start.scss'
import { useDialog } from '@ui/hooks/dialog.hooks'
import { DialogType } from '@persistence/store/ui/ui.state'

const Start: FunctionComponent = () => {
    const gameInfo = useContext(GameInfoContext)

    const { fetchAndSave: fetchAndSaveChangelogs } = useActions().changelogs
    const changelogs = useSelector<State, ChangelogsState>(state => state.changelogs)
    useMountEffect(() => {
        fetchAndSaveChangelogs()
    })

    const { attemptLoad } = useActions().savegames
    const { exit, openInBrowser } = useActions().window
    const { showDialog } = useDialog()

    const attemptLoadPreviousSavegame = () => {
        pipe(
            attemptLoad,
            fold(() => of(showDialog(DialogType.StartDateSelection)), () => of(alert('Coming soon.'))),
        )()
    }

    return (
        <Layout
            left={
                <FullSizeSidebar
                    className="main-menu"
                    header={
                        <>
                            <h1 className="logo">Duets</h1>

                            <div className="saves-buttons">
                                <PlayButton onClick={attemptLoadPreviousSavegame} />
                            </div>
                        </>
                    }
                    footer={
                        <>
                            <h3>v{gameInfo.version}</h3>
                            <div>
                                <a className="external-url" onClick={() => openInBrowser(gameInfo.sourceCodeUrl)}>
                                    source code
                                </a>
                                <a className="external-url" onClick={() => openInBrowser(gameInfo.homepageUrl)}>
                                    homepage
                                </a>
                            </div>
                        </>
                    }
                    navButton={NavButton.Close}
                    onNavButtonClick={exit}
                />
            }
            right={
                <div className="changelog">
                    {changelogs === 'loading' ? (
                        'Loading...'
                    ) : changelogs instanceof Error ? (
                        'There was an error loading the changelog'
                    ) : (
                        <Changelog changelogList={changelogs} />
                    )}
                </div>
            }
        />
    )
}

export default Start
