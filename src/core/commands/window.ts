import { IO } from 'fp-ts/lib/IO'
import WindowInteractor from '@core/interfaces/window'

export interface WindowCommands {
    exit: IO<void>
    openInBrowser(url: string): IO<void>
}

export default (windowInteractor: WindowInteractor): WindowCommands => ({
    exit: () => windowInteractor.exit(),
    openInBrowser: (url: string) => windowInteractor.openInBrowser(url),
})
