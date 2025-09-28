import React, { useContext, useEffect, useState } from "react";
import axios from "axios";
import { Background } from "../components/Background";
import { LobbyScreenButton } from "../components/Buttons";
import { LobbyDisplay, LobbyListItem } from "../components/LobbyListItem";
import AuthorizationContext from "../contexts/AuthorizationContext";
import { Navigate, useNavigate } from "react-router-dom";
export default function LobbySelectScreen()
{   
    const {APIUrl, user} = useContext(AuthorizationContext);
    const [lobbyList, setLobbyList] = useState([]);

    const navigate = useNavigate()

    useEffect(() => {
        refresh()
    }, [])

    const refresh = async () => {
        const route = "/Lobby/GetLobbyList"
        await axios.get(APIUrl + route)
        .then(response => {
            console.log(response)
            setLobbyList(response.data)
        })
        .catch(err => {
            console.log(err)
        })
        console.log(lobbyList)
    }

    const createLobby = async (e) => {
        e.preventDefault()

        if(user.id == -1)
        {
            return;
        }

        const route = "/Lobby/CreateLobby"
        var lobby = {
            name: `${user.username}'s Lobby`,
            maxPlayers: 4,
            currentPlayers: 1,
            quizID: null,
            creatorID: user.id,
            description: `${user.username}'s quiz lobby! Join now!`,
            isPrivate: false,
            password: null,
            status: 0,
            readyPlayers: 0
        }

        await axios.post(APIUrl + route, lobby)
        .then(response => {
            console.log(response)
        })
        .catch(err => {
            console.log(err)
        })

        refresh()
    }
    
    return (
        <>
            <Background>
                <div className="flex flex-row rounded-lg w-[90%] h-[90%] bg-slate-700 mx-auto my-auto" >
                    <div className="flex flex-col rounded-lg h-[90%] w-1/4 my-auto mx-12 bg-slate-600">
                        <LobbyScreenButton className = "mx-auto my-auto w-3/4 h-1/4 rounded-2xl bg-green-400 shadow-green-500 shadow-md hover:bg-green-500 hover:shadow-green-600 hover:scale-105 transition duration-10" onClick = {(e) => createLobby(e)} text="Create Lobby" />
                        <LobbyScreenButton className = "mx-auto my-auto w-3/4 h-1/4 rounded-2xl bg-purple-400 shadow-purple-500 shadow-md hover:bg-purple-500 hover:shadow-purple-600 hover:scale-105 transition duration-10" onClick = {refresh} text="Refresh" />
                        <LobbyScreenButton className = "mx-auto my-auto w-3/4 h-1/4 rounded-2xl bg-rose-400 shadow-rose-500 shadow-md hover:bg-rose-500 hover:shadow-rose-600 hover:scale-105 transition duration-10" onClick = {() => navigate("../")} text="Back to Title" />
                    </div>
                    <div className="flex flex-col rounded-lg overflow-auto h-[90%] w-[65%] my-auto mx-12 bg-slate-500">
                        {
                            lobbyList?.map(lobby => (
                                <>
                                    {lobby.status == 0 && <LobbyListItem userID={user.id} lobbyID={lobby.id} name={lobby.name} description={lobby.description} currentPlayers={lobby.currentPlayers} maxPlayers={lobby.maxPlayers} isPrivate={lobby.isPrivate} />}
                                </>
                            ))
                        }
                    </div>
                </div>
            </Background>
        </>
    )
}