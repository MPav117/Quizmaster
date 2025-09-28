import React from 'react';
import './App.css';
import MainScreen from './screens/MainScreen';
import LoginScreen from './screens/LoginScreen';
import LobbySelectScreen from './screens/LobbySelectScreen';
import RegisterScreen from './screens/RegisterScreen';
import CreateQuiz from './screens/CreateQuiz';

import AuthorizationContext from './contexts/AuthorizationContext';
import { useState } from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import LobbyScreen from './screens/LobbyScreen';
import Endcard from './screens/Endcard';
import MyQuizzes from './screens/MyQuizzes';

function App() {

  const [user, setUser] = useState({
    id: -1,
    username: "",
    level: 0,
    experience: 0,
    expPercentage: 0,
    jwtToken: "",
    picture: "",
  })
  const APIUrl = "http://127.0.0.1:5107/API";

  var storageUser = localStorage.getItem('QuizmasterUser');

  if (user.id === -1 && storageUser) {
    var storageUserJson = JSON.parse(storageUser);
    console.log(storageUserJson)
    if(storageUserJson.id != -1)
    {
      setUser(storageUserJson);
    }
  }

  const value = { APIUrl, user, setUser };

  return (
    <>
      <AuthorizationContext.Provider value={value}>
        <BrowserRouter>
          <Routes>
            <Route path="/" element={<MainScreen />}></Route>
            <Route path="/login" element={<LoginScreen />}></Route>
            <Route path="/register" element={<RegisterScreen />}></Route>
            <Route path="/selectLobby" element={<LobbySelectScreen />}></Route>
            <Route path="/quizzes" element={<MyQuizzes />}></Route>
            <Route path="/lobby/:lobbyID" element={<LobbyScreen />}></Route>
          </Routes>
        </BrowserRouter>
      </AuthorizationContext.Provider>
    </>
  );
}

export default App;
