import React from "react";

export default function DisplayLobbyInfo({lobby, quiz})
{
    return(
        <div className="text-white text-lg w-full h-full">
            <p className="mx-4 my-4">{`Lobby ID: ${lobby.id}`}</p>
            <p className="mx-4 my-4">Lobby Name: {lobby.name}</p>
            <p className="mx-4 my-4">Description: {lobby.description}</p>
            <p className="mx-4 my-4">Current Players: {lobby.currentPlayers}</p>
            <p className="mx-4 my-4">Ready Players: {lobby.readyPlayers}</p>
            <p className="mx-4 my-4">Max Players: {lobby.maxPlayers}</p>
            {quiz != null && <p className="mx-4 my-4">Quiz: {quiz.name}</p>}
            {quiz != null && <p className="mx-4 my-4">Question count: {quiz.questionCount}</p>}
        </div>
    )
}
