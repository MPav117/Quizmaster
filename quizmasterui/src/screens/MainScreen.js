import React, { useEffect } from 'react';
import axios from 'axios';
import { useState } from 'react';
import { useContext } from 'react';
import {GenericButton, MainScreenButton} from '../components/Buttons';
import AuthorizationContext from '../contexts/AuthorizationContext';
import default_profile_picture from '../resources/default_profile_picture.png'
import { useNavigate } from 'react-router-dom';
import { Background } from '../components/Background';

function MainScreen() {
  const navigate = useNavigate()
  const [isLogin, setLogin] = useState(false)
  const { APIUrl, user, setUser } = useContext(AuthorizationContext)

  const handlePlayClick = () => {
    navigate("/selectLobby")
  }
  
  const handleLoginClick = () => {
    if(user.id == -1)
    {
      navigate("/Login")
    }
    else
    {
      console.log(user)
      const resetUser = {
        id: -1,
        username: "",
        level: 0,
        experience: 0,
        jwtToken: "",
        profilePicture: ""
      }
      localStorage.removeItem("QuizmasterUser")
      setUser(resetUser)
      setLogin(false)
    }
  }

  const handleCreateClick = () => {
    navigate("/quizzes")
  }

  useEffect(() => {
    if(user.id != -1) {
      setLogin(true)
      handleRefresh()
    }
  }, [])

  const handleRefresh = async () => {
    await axios.get(APIUrl + `/Auth/GetUser/${user.id}`)
        .then(response => {
              console.log(response)
              const refreshUser = {
                id: user.id,
                username: user.username,
                level: response.data.level,
                experience: response.data.experience,
                jwtToken: user.jwtToken,
                profilePicture: ""
              }
              setUser(refreshUser)
              localStorage.removeItem('QuizmasterUser')
              localStorage.setItem('QuizmasterUser', JSON.stringify(refreshUser));
        })
  }
  return (
    <Background>
    <div className="flex flex-row h-1/5 w-4/5 mx-auto my-auto justify-center rounded-3xl">
      <p className="text-9xl my-auto text-orange-400 rotate-30">Quiz</p>
      <p className="text-9xl my-auto text-purple-400 -rotate-30">Master</p>
    </div>

    <div className ="flex flex-row h-40 w-2/5 mx-auto my-5 justify-center rounded-3xl bg-slate-700">
      {isLogin && 
        <div className="flex flex-row h-full w-full justify-center">
            <img src={default_profile_picture}></img>
            <div className="flex flex-col h-full w-2/5">
              <p className="text-2xl mx-auto my-auto text-white">{`${user.username}`}</p>
              <p className="text-2xl mx-auto text-white">{`Level: ${user.level}`}</p>
              <p className="text-2xl mx-auto my-auto text-white">{`Exp: ${user.experience}`}</p>
              <div className="flex flex-row w-full h-1/5 bg-white border-black border-x-4 border-y-4">
                <div style={{width: `${user.expPercentage}%`}} className={"flex bg-lime-400"}></div>
              </div>
            </div>
        </div>
      }
      {!isLogin &&
        <div className="mx-auto my-auto">
          <p className="text-4xl mx-auto my-auto text-white">User is not logged in.</p>
        </div>
      }
    </div>
    <div className="flex flex-row h-1/5 w-4/5 mx-auto my-auto justify-center rounded-3xl bg-slate-700">
      <GenericButton onClick={handlePlayClick} className={(user.id == -1 ? "bg-gray-500 shadow-gray-700" : "bg-cyan-500 shadow-cyan-700 hover:bg-cyan-600 hover:shadow-cyan-800") + " mx-auto my-auto w-1/5 h-4/5 rounded-lg shadow-md text-5xl"} disabled={user.id == -1} text="Play"></GenericButton>
      <GenericButton onClick={handleCreateClick} className={(user.id == -1 ? "bg-gray-500 shadow-gray-700" : "bg-orange-500 shadow-orange-700 hover:bg-orange-600 hover:shadow-orange-800") + " mx-auto my-auto w-1/5 h-4/5 rounded-lg shadow-md text-5xl"} disabled={user.id == -1} text="Quizzes"></GenericButton>
      <GenericButton onClick={handleLoginClick} className="mx-auto my-auto w-1/5 h-4/5 rounded-lg shadow-md bg-purple-500 shadow-purple-700 hover:bg-purple-600 hover:shadow-purple-800 text-5xl" text={user.id == -1 ? "Login" : "Logout"}></GenericButton>
    </div>
  </Background>
  );
}

export default MainScreen;