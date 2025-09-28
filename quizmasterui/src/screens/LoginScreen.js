import { Button, TextField } from "@mui/material";
import React, { useContext, useState } from "react";
import AuthorizationContext from "../contexts/AuthorizationContext";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import { CalculateExperiencePercentage } from "../utils/Experience";

function LoginScreen() {
    const [email, setEmail] = useState("")
    const [password, setPassword] = useState("")
    const { APIUrl, user, setUser } = useContext(AuthorizationContext)
    let navigate = useNavigate()
    
    const login = async (event) => {
        event.preventDefault()
        await axios.post(APIUrl + "/Auth/Login", {
            email: email,
            password: password
        })
        .then(function (response) {
            console.log(response)
            if(response.status == 200)
            {
                var data = response.data;
                setUser(data.user)
                
                var now = new Date();
                now.setHours(now.getHours() + 6);
                localStorage.setItem('QuizmasterUser', JSON.stringify(data.user));
                localStorage.setItem('QuizmasterExpiryDate', now);
                
                navigate("/");
            }
        })
        .catch(function (error) {
            console.log(error)
        })
    }

    const navigateToRegister = () => {
        navigate("/register");
    }

    return (
        <div className="flex flex-col h-dvh justify-center bg-gradient-to-b from-slate-600 to-slate-400">
            <div className="flex flex-col h-[600px] w-[500px] bg-slate-600 mx-auto my-auto">
                <div className="flex flex-row h-1/5 w-full p-12">
                    <p className="text-4xl font-bold text-orange-400 ml-auto">Quiz</p>
                    <p className="text-4xl font-bold text-purple-400 mr-auto">Master</p>
                </div>
                <div className="flex flex-col h-3/4 w-full p-12">
                    <TextField className="bg-gray-100" id="outlined-basic" label="EMail" variant="filled" onChange={(event) => {
                        setEmail(event.target.value)
                    }}/>

                    <div className="h-12"></div>
                    <TextField className="bg-gray-100" id="outlined-basic" label="Password" variant="filled" onChange={(event) => {
                        setPassword(event.target.value)
                    }}/>

                    <div className="h-12"></div>
                    <div className="flex flex-row w-full justify-center">
                        <Button className="w-1/4" variant="contained" onClick={login}>Login</Button>
                    </div>

                    <div className="h-12"></div>
                    <div className="flex flex-row h-fit w-fit">
                        <p className="text-white text-2xl mr-8">Don't have an account?</p>
                        <Button className="w-1/3 mx-auto" variant="contained" onClick={navigateToRegister}>Register</Button>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default LoginScreen