import React, { useState, useContext } from "react";
import TextInput from "./TextInput";
import { useEffect } from "react";
import { GenericButton } from "./Buttons";
import AuthorizationContext from "../contexts/AuthorizationContext";
import { NotifyOfLobbyUpdate } from "../signalr/signalr";
import axios from "axios";

export default function EditLobbyInfo({lobby, quiz})
{   
    const {APIUrl} = useContext(AuthorizationContext)

    const maxPlayerCount = [2, 3, 4];

    const [name, setName] = useState("")
    const [description, setDescription] = useState("")
    const [maxPlayers, setMaxPlayers] = useState(4);

    useEffect(() => {
        setName(lobby.name)
        setDescription(lobby.description)
        setMaxPlayers(lobby.maxPlayers)
    }, [])

    const updateQuiz = async () => {
        const updateQuizPath = "/Lobby/UpdateLobby"
        
        var lobbyToUpdate = structuredClone(lobby)

        lobbyToUpdate.name = name;
        lobbyToUpdate.description = description;
        lobbyToUpdate.maxPlayers = parseInt(maxPlayers);
        lobbyToUpdate.isPrivate = false;

        if(quiz !== null && quiz !== undefined)
        {
            lobbyToUpdate.quizID = quiz.id;
        }

        await axios.put(APIUrl + updateQuizPath, lobbyToUpdate)
        .then(response => {
            console.log(response)
            NotifyOfLobbyUpdate(lobbyToUpdate.id)
        })
        .catch(err => {
            console.log(err)
        })
    }
    return (
        <div className="flex flex-col w-full h-full text-lg">

            <p className="text-white mx-4 my-2">{`Lobby ID: ${lobby.id}`}</p>
            
            <TextInput className={"w-[80%] mx-4 my-2"} value={name} onChange={(e) => setName(e.target.value)}>Lobby Name:</TextInput>
            <TextInput className={"w-[80%] mx-4 my-2"} value={description} onChange={(e) => setDescription(e.target.value)}>Description:</TextInput>
            
            <p className="text-white mx-4 my-2">Current Players: {lobby.currentPlayers}</p>
            <p className="text-white mx-4 my-2">Ready Players: {lobby.readyPlayers}</p>

            <label className="mx-4 my-2 text-white">
                Max Players:
                <select className="mx-2 w-[20%] text-black" value = {maxPlayers} onChange = {e => setMaxPlayers(e.target.value)}>
                    {
                        maxPlayerCount.map(count => (
                            <option value={count}>{count}</option>
                        ))
                    }
                </select>
            </label>
            {quiz != null && <p className="text-white mx-4 my-2">Quiz: {quiz.name}</p>}
            {quiz != null && <p className="text-white mx-4 my-2">Question count: {quiz.questionCount}</p>}

            <GenericButton className="flex mx-auto mt-auto mb-4 h-24 w-48 bg-green-500 shadow-green-700 shadow-md hover:bg-green-600 hover:shadow-green-800" text="Update Quiz" onClick={updateQuiz}></GenericButton>
        </div>
    )
}