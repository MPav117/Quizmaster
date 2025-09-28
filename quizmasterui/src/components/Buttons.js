import React from "react"

export function MainScreenButton({onClick, text, className, disabled, children}) {
    return (
        <div className={((className === undefined && disabled === false) ? "" : className) + `flex mx-auto my-auto rounded-3xl ${disabled && `bg-gray-400 shadow-gray-200 hover:bg-gray-600 hover:shadow-gray-400`} shadow-md hover:scale-105 transition duration-10`}>
            <button className='flex h-full w-full' onClick={onClick} disabled={disabled === undefined ? false : disabled}>
                <p className="text mx-auto lg:text-5xl md:text-3xl sm:text-xl my-auto text-white">{text}</p>
            </button>
        </div>
    )
}

export function LobbyScreenButton({onClick, text, className, disabled, children}) {
    return (
        <div className = {className === undefined ? "" : className}>
            <button className = 'flex h-full w-full' onClick = {onClick} disabled = {disabled === undefined ? false : disabled}>
                <p className = "mx-auto lg:text-4xl md:text-xl sm:text-xl short:text-2xl my-auto text-white">
                    {text === undefined ? "" : text}
                </p>
            </button>
        </div>
    )
}

export function GenericButton({onClick, text, className, disabled, children}) {
    return (
        <div className = {className === undefined ? "" : className}>
            <button className = 'flex h-full w-full' onClick = {onClick} disabled={disabled === undefined ? false : disabled}>
                <p className = "mx-auto my-auto text-white">
                    {text === undefined ? "" : text}
                </p>
            </button>
        </div>
    )
}