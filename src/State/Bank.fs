namespace State

module Bank =
    open Aether
    open Entities

    let transfer account transaction =
        let transactionsLens =
            Lenses.FromState.BankAccount.transactionsOf_ account

        let addTransaction = List.append [ transaction ]

        Optic.map transactionsLens addTransaction
