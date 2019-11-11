import React from 'react'
import ReactDOM from 'react-dom'
import { BrowserRouter } from 'react-router-dom'
import { Provider } from 'react-redux'
import { AnyAction, Store } from 'redux'
import App from './app'
import { GameInfo } from '@ui/types/game-info'
import { CommandsContext, Injections } from '@ui/contexts/injections.context'
import { GameInfoContext } from '@ui/contexts/game-info.context'
import { InjectionsContext } from './contexts/injections.context'
import './index.scss'
import { Commands } from '@core/commands/commands'

/**
 * Renders the app with the game information.
 */
export default (appInfo: GameInfo, commands: Commands, injections: Injections, store: Store<any, AnyAction>) => {
    const root = document.getElementById('root')

    if (root) {
        root.dataset.theme = 'dark'
    }

    return ReactDOM.render(
        <GameInfoContext.Provider value={appInfo}>
            <CommandsContext.Provider value={commands}>
                <InjectionsContext.Provider value={injections}>
                    <Provider store={store}>
                        <BrowserRouter>
                            <App />
                        </BrowserRouter>
                    </Provider>
                </InjectionsContext.Provider>
            </CommandsContext.Provider>
        </GameInfoContext.Provider>,
        root,
    )
}
