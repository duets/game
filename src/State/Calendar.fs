namespace State

module Calendar =
    open Aether
    open Entities

    let setTime time =
        Optic.set Lenses.State.today_ time
