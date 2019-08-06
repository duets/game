import React from 'react'
import ReactDOM from 'react-dom'
import { HashRouter } from 'react-router-dom'
import { Provider } from 'react-redux'
import './index.scss'
import App from './app'
import { GameInfo } from '@ui/types/game-info'
import { Injections } from '@ui/contexts/injections.context'
import { GameInfoContext } from '@ui/contexts/game-info.context'
import { InjectionsContext } from './contexts/injections.context'
import { AnyAction, Store } from 'redux'

/**
 * Renders the app with the game information.
 */
export default (appInfo: GameInfo, injections: Injections, store: Store<any, AnyAction>) => {
    return ReactDOM.render(
        <GameInfoContext.Provider value={appInfo}>
            <InjectionsContext.Provider value={injections}>
                <Provider store={store}>
                    <HashRouter>
                        <App />
                    </HashRouter>
                </Provider>
            </InjectionsContext.Provider>
        </GameInfoContext.Provider>,
        document.getElementById('root'),
    )
}
