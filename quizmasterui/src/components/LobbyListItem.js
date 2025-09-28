import React from "react"
import { GenericButton } from "./Buttons"
import { Navigate, useNavigate } from "react-router-dom"

export function LobbyListItem ({userID, lobbyID, name, currentPlayers, maxPlayers, description, isPrivate})
{
    const navigate = useNavigate()

    const joinLobby = () => {
        navigate(`../lobby/${lobbyID}`)
    }
    
    return(
        <div className="flex flex-row mx-8 my-4 h-36 w-[90%] rounded-lg bg-blue-400">
            <div className="flex flex-auto my-auto mx-4 h-2/3 bg-blue-300 rounded-xl">
                <div className="flex flex-auto mx-2 h-full w-1/6 my-auto">
                    <p className="my-auto text-xl">{name}</p>
                </div>
                <div className="flex flex-auto mx-2 h-full w-1/12 my-auto">
                    <p className="my-auto text-2xl">{currentPlayers}/{maxPlayers}</p>
                </div>
                <div className="flex flex-auto mx-2 h-full w-1/2 my-auto">
                    <p className="my-auto line-clamp-3 ">{description}</p>
                </div>
            </div>
            <GenericButton onClick={joinLobby} text="Join Lobby" className = "h-4/5 w-1/5 my-auto mx-2 rounded-md shadow-md bg-orange-400 shadow-orange-600 hover:bg-orange-500 hover:shadow-orange-700 hover:scale-105 transition duration-10" />
        </div>
    )
}