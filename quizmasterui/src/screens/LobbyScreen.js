import React, { useCallback, useEffect } from "react";
import AuthorizationContext from "../contexts/AuthorizationContext";
import axios from "axios";

import { Background } from "../components/Background";
import { useNavigate, useParams } from "react-router-dom";

import { useContext } from "react";
import { useState } from "react";

import { startConnection, StartQuiz, BuzzIn, connectionState, stopConnection, joinLobby, Ready, Unready, AnswerQuestion } from "../signalr/signalr";
import { onLobbyUpdated, onUserChange, onQuizEnded, onQuizStart, onGetNextQuestion, onBuzzRecieved, onIncorrectAnswerRecieved, onCorrectAnswerRecieved, onTimeoutRecieved, onTimedOut} from "../signalr/signalr";

import DisplayLobbyInfo from "../components/DisplayLobbyInfo";
import EditLobbyInfo from "../components/EditLobbyInfo";
import { GenericButton } from "../components/Buttons";
import DisplayUsersInLobbyItem from "../components/DisplayUsersInLobbyItem";

import Quiz from "./Quiz";
import { QuizListItem } from "../components/QuizListItem";
import Endcard from "./Endcard";

export default function LobbyScreen()
{   
    const {lobbyID} = useParams()
    const {APIUrl, user} = useContext(AuthorizationContext);

    const [quizStarted, setQuizStarted] = useState(false);
    const [quizEnded, setQuizEnded] = useState(false);

    const [lobby, setLobby] = useState(null)
    const [quiz, setQuiz] = useState(null)
    const [sessions, setSessions] = useState([])
    const [question, setQuestion] = useState(null)
    const [quizzes, setQuizzes] = useState([])

    const [userIsReady, setUserIsReady] = useState(false)
    const [isEditingLobby, setIsEditingLobby] = useState(false)
    const [isSelectingQuiz, setIsSelectingQuiz] = useState(false)

    const [inputDisabled, setInputDisabled] = useState(true)
    const [buzzDisabled, setBuzzDisabled] = useState(false)
    const [displayAnswerDisabled, setDisplayAnswerDisabled] = useState(false);

    const [buzzedID, setBuzzedID] = useState(-1);

    const [timeLeft, setTimeLeft] = useState(0);
    const [timer, setTimer] = useState(null);
    const [timerEnabled, setTimerEnabled] = useState(false);

    const navigate = useNavigate();

    useEffect(() => {
        getLobby()
    }, [])

    useEffect(() => {
        if(timerEnabled == false && timer != null) {
            clearInterval(timer);
            setTimer(null);
        }
    }, [timerEnabled])

    const getLobby = async () => {
        const route = "/Lobby/GetLobby/"

        await axios.get(APIUrl + route + lobbyID)
        .then(async response => {
            
            setLobby(response.data)
            if(response.data.quiz != null && response.data.quiz != undefined)
            {
                setQuiz(response.data.quiz)
            }

            console.log(response.data)
            
            await startConnection(user.jwtToken)

            onUserChange(async () => {

                console.log("Message recieved - User changed!")
                const callbackRouteLobby = "/Lobby/GetLobby/"
                const callbackRouteSessions = "/Lobby/GetLobbySessions/"

                await axios.get(APIUrl + callbackRouteLobby + lobbyID)
                .then(async response => {

                    // response.data is lobby
                    setLobby(response.data)
                    console.log(response)
                    
                    if(response.data.quiz != null && response.data.quiz != undefined)
                    {
                        setQuiz(response.data.quiz)
                    }

                    await axios.get(APIUrl + callbackRouteSessions + lobbyID)
                    .then(innerResponse => {
                        console.log(innerResponse.data)
                        setSessions(innerResponse.data)
                        console.log(innerResponse)

                        var userSession = innerResponse.data.find((session) => session.userID == user.id)
                        console.log(userSession)
                        setUserIsReady(userSession.ready)
                    })
                    .catch(err => {
                    console.log(err)
                    })
                })
                .catch(err => {
                    console.log(err)
                })
            })

            onLobbyUpdated(async () => {
                console.log("Message recieved - Lobby updated!")
                const callbackRouteLobby = "/Lobby/GetLobby/"
                await axios.get(APIUrl + callbackRouteLobby + lobbyID)
                .then(response => {
                    setLobby(response.data)
                    setIsEditingLobby(false)
                    console.log(response)
                })
                .catch(err => {
                    console.log(err)
                })
            })
            
            onQuizStart(async () => {
                console.log("Message recieved - Quiz started!")
                const callbackRouteSessions = "/Lobby/GetLobbySessions/"
                await axios.get(APIUrl + callbackRouteSessions + lobbyID)
                    .then(innerResponse => {
                        console.log(innerResponse.data)
                        setSessions(innerResponse.data)
                        setQuizStarted(true);
                    })
            })

            onBuzzRecieved((buzzerID) => {
                console.log("Message recieved - Buzz recieved!")
                if(user.id == buzzerID)
                {
                    setInputDisabled(false)
                }
                setBuzzDisabled(true)
                setBuzzedID(buzzerID)
            })

            onIncorrectAnswerRecieved(async (wrongUserID) => {
                setBuzzedID(-1)
                console.log("Message recieved - Incorrect answer recieved!")
                const callbackRouteSessions = "/Lobby/GetLobbySessions/"
                await axios.get(APIUrl + callbackRouteSessions + lobbyID)
                    .then(innerResponse => {
                        console.log(innerResponse.data)
                        setSessions(innerResponse.data)
                        innerResponse.data.forEach(element => {
                            console.log(element)
                            if(element.userID == user.id)
                            {
                                if(element.incorrect == true)
                                {
                                    console.log("I am disabled!")
                                    setBuzzDisabled(true)
                                }
                                else
                                {
                                    console.log("I am enabled!")
                                    setBuzzDisabled(false)
                                }   
                            }
                        })
                    }
                )
                setInputDisabled(true)
            })
            
            onCorrectAnswerRecieved(async (correctUserID) => {
                handleClearTime();
                setBuzzedID(-1)
                
                console.log("Message recieved - Correct answer recieved!")
                const callbackRouteSessions = "/Lobby/GetLobbySessions/"
                await axios.get(APIUrl + callbackRouteSessions + lobbyID)
                    .then(innerResponse => {
                        console.log(innerResponse.data)
                        setSessions(innerResponse.data)
                    }
                )

                setDisplayAnswerDisabled(false);
                setInputDisabled(true)
                setBuzzDisabled(true)
            })

            onGetNextQuestion(async (questionID) => {
                handleClearTime();
                console.log("Message recieved - Get next question!")
                const questionRoute = "/Quiz/GetQuizQuestion/"
                
                setDisplayAnswerDisabled(true)
                console.log("Got next question!")
                setBuzzDisabled(false)
                setInputDisabled(true)

                await axios.get(APIUrl + questionRoute + questionID.toString())
                .then(result => {
                    console.log(result)
                    setQuestion(result.data)
                })
                .catch(err => {
                    console.log(err)
                })
            })
            
            onTimeoutRecieved((seconds) => {
                console.log("Message recieved - Set timer!")
                handleSetTime(seconds);
            })

            onTimedOut(async () => {
                setTimerEnabled(false);
                console.log("Message recieved - Times out!")
                const callbackRouteSessions = "/Lobby/GetLobbySessions/"

                await axios.get(APIUrl + callbackRouteSessions + lobbyID)
                    .then(innerResponse => {
                        console.log(innerResponse.data)
                        setSessions(innerResponse.data)
                    })
                
                var newLobby = lobby.copy()
                newLobby.readyPlayers = 0
                setLobby(newLobby)

                setDisplayAnswerDisabled(false);
                setBuzzDisabled(true);
                setInputDisabled(true);
            })
            
            onQuizEnded(async () => {
                setQuizEnded(true)
                console.log("Message recieved - Quiz ended!")
                const callbackRouteSessions = "/Lobby/GetLobbySessions/"
                await axios.get(APIUrl + callbackRouteSessions + lobbyID)
                    .then(innerResponse => {
                        console.log(innerResponse.data)
                        setSessions(innerResponse.data)
                    }
                )
                .catch(err => {
                    console.log(err)
                })

                const callbackRouteLobby = "/Lobby/GetLobby/"
                await axios.get(APIUrl + callbackRouteLobby + lobbyID)
                .then(response => {
                    setLobby(response.data)
                    setIsEditingLobby(false)
                    console.log(response)
                })
                .catch(err => {
                    console.log(err)
                })

                setTimerEnabled(false);
                setDisplayAnswerDisabled(true);
                setBuzzDisabled(true);
                setInputDisabled(true);
                setUserIsReady(false);
            })

            await joinLobby(response.data.id, user.id)

            if(response.data.creatorID == user.id)
            {
                const quizListRoute = "/Quiz/GetAllQuizzesCreatedByUser/"
                await axios.get(APIUrl + quizListRoute + user.id.toString())
                .then(response => {
                    console.log(response)
                    setQuizzes(response.data)
                })
                .catch(err => {
                    console.log(err)
                })
            }
        })
    }

    const handleSetTime = (seconds) => {
        setTimeLeft(seconds);
        var localTimeLeft = seconds;
        var interval = setInterval(() => {
            localTimeLeft = localTimeLeft - 1;
            setTimeLeft(localTimeLeft);
            if(localTimeLeft == 0)
            {
                clearInterval(interval);
            }
        }, 1000);
        setTimer(interval);
        setTimerEnabled(true);
    }

    const handleClearTime = () => {
        setTimerEnabled(false);
    };

    const handleSwitch = () => {
        setIsEditingLobby(!isEditingLobby)
    }

    const handleStartQuiz = async () => {
       StartQuiz(lobby.id, quiz.id) 
    }

    const checkState = () => {
        console.log(connectionState())
    }

    const handleReadyClick = () => {
        if(userIsReady == true) {
            Unready()
        }

        if(userIsReady == false) {
            Ready()
        }
    }

    const handleBuzz = () => {
        setBuzzDisabled(true)
        BuzzIn(lobby.id, question.id, user.id)
    }

    const handleInput = (answer) => {
        setInputDisabled(true)
        setBuzzDisabled(true)
        AnswerQuestion(lobby.id, question.id, user.id, answer)
    }

    const handleGoBack = () => {
        stopConnection()
        navigate("../selectLobby");
    }

    const handleReturnToLobby = () => {
        setQuizStarted(false);
        setQuizEnded(false);
    }

    return (
        <>
            <Background>
                {(quizStarted == false && quizEnded == false) &&
                <div className="flex flex-row w-[90%] h-[90%] bg-slate-700 mx-auto my-auto">
                    <div className="flex flex-col w-[30%] h-[90%] mx-auto my-auto bg-slate-600">
                        <div className="flex flex-row w-full">
                            <GenericButton className="flex mr-auto ml-2 my-2 px-2 py-2 w-fit bg-purple-500 shadow-purple-700 shadow-md hover:bg-purple-600 hover:shadow-purple-800" onClick={handleGoBack} text={`Leave Lobby`}></GenericButton>
                            {lobby && 
                            <>
                                {(lobby.creatorID == user.id) && <GenericButton className="flex ml-auto mr-2 my-2 px-2 py-2 w-fit bg-amber-600 shadow-amber-800 shadow-md hover:bg-amber-700 hover:shadow-amber-900" onClick={handleSwitch} text={`Switch to ${isEditingLobby ? "view" : "edit"}`}></GenericButton> }
                            </>
                            }
                        </div>
                        {lobby && 
                            <div className="flex w-full h-full">
                                {lobby && !isEditingLobby && 
                                    <div className="flex flex-col w-full h-full">
                                        <DisplayLobbyInfo lobby={lobby} quiz={quiz} />
                                        <GenericButton className={(userIsReady ? "bg-blue-500 shadow-blue-700 hover:bg-blue-600 hover:shadow-blue-800" : "bg-red-500 shadow-red-700 hover:bg-red-600 hover:shadow-red-800") + "flex flex-auto mx-auto my-4 w-[90%] h-24 text-white shadow-md "} text={userIsReady ? "Ready" : "Unready"} onClick={handleReadyClick}></GenericButton>
                                        {lobby.creatorID == user.id && <GenericButton disabled={(lobby.readyPlayers < lobby.currentPlayers || quiz == null)} className={((lobby.readyPlayers < lobby.currentPlayers || quiz == null) ? "bg-gray-500 shadow-gray-700 " :"bg-red-500 shadow-red-700 hover:bg-red-600 hover:shadow-red-800 ") + "shadow-md flex flex-auto mx-auto my-4 w-[90%] h-24 text-white"} text={"Start Quiz"} onClick={handleStartQuiz}></GenericButton>}
                                    </div>}
                                {lobby && isEditingLobby && <EditLobbyInfo lobby={lobby} quiz={quiz} />}
                            </div>
                        }   
                    </div>
                    <div className="flex flex-col w-[60%] h-[90%] mx-auto my-auto bg-slate-600">
                        {!isEditingLobby &&
                            sessions.map((session, index) => (
                                <div key={index} className="w-full h-full">
                                    <DisplayUsersInLobbyItem session={session} index={index}></DisplayUsersInLobbyItem>
                                </div>
                            ))
                        }
                        {(isEditingLobby && quizzes.length > 0) &&
                            <div>
                                {
                                    quizzes.map((quiz) => (
                                        <QuizListItem quiz={quiz} onClick={() => setQuiz(quiz)}></QuizListItem>
                                    ))
                                }
                            </div>
                        }
                    </div>
                </div>
                }
                { (quizStarted == true && quizEnded == false) && 
                    <div className="flex h-full w-full">
                        <Quiz lobby={lobby} quiz={quiz} sessions={sessions} quizQuestion={question} inputDisabled={inputDisabled} handleBuzz={handleBuzz} handleInput={handleInput} buzzDisabled={buzzDisabled} displayAnswerDisabled={displayAnswerDisabled} timeLeft={timeLeft}></Quiz>
                    </div>
                }
                { (quizStarted == true && quizEnded == true) && 
                    <div className="flex h-full w-full">
                        <Endcard sessions={sessions} handleReturnToLobby={handleReturnToLobby}></Endcard>
                    </div>
                }
            </Background>
        </>
    );
}