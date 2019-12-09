import React, { FunctionComponent, ReactNode } from 'react'
import '@ui/styles/full-size.sidebar.scss'
import { NavButton } from '@ui/components/buttons/nav/nav-button-type'
import BackButton from '@ui/components/buttons/nav/back.button'
import CloseButton from '@ui/components/buttons/nav/close.button'

type FullSizeSidebarProps = {
    className: string
    header?: ReactNode
    footer?: ReactNode
    navButton: NavButton
    onNavButtonClick: () => void
}

const FullSizeSidebar: FunctionComponent<FullSizeSidebarProps> = props => {
    const navButton = () => {
        switch (props.navButton) {
            case NavButton.Hide:
                return <></>
            case NavButton.Close:
                return <CloseButton className="nav" onClick={props.onNavButtonClick} />
            case NavButton.Back:
                return <BackButton className="nav" onClick={props.onNavButtonClick} />
        }
    }

    return (
        <div className={`full-size-sidebar ${props.className}`}>
            <header>
                {navButton()}
                {props.header}
            </header>

            <footer>{props.footer}</footer>
        </div>
    )
}

export default FullSizeSidebar
