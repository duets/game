import { tryCatch } from 'fp-ts/lib/TaskEither'
import RemoteDatabase from '@core/interfaces/database/remote.database'

const githubDatabase: RemoteDatabase = {
    // TODO: Implement.
    getCities: tryCatch(() => new Promise(resolve => resolve([])), error => new Error(String(error))),
}

export default githubDatabase
