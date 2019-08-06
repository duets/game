/**
 * This file will serve as the entry point of the app, here will be the configuration of our dependency injection
 * container and the initialization core parts of the game like the UI.
 */
import InitializeUi from '@ui/main'
import { version, homepage } from '../package.json'
import Store from '@persistence/store/store'
import { GameInfo } from '@ui/types/game-info.js'
import Injections from './injections'

const gameInfo: GameInfo = {
    homepageUrl: homepage,
    sourceCodeUrl: homepage,
    version,
}

InitializeUi(gameInfo, Injections, Store)
