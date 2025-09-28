import { Button, TextField } from "@mui/material";
import React, { useContext, useState } from "react";
import AuthorizationContext from "../contexts/AuthorizationContext";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import { CalculateExperiencePercentage } from "../utils/Experience";

function RegisterScreen() {
    const [username, setUsername] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const { APIUrl, user, setUser } = useContext(AuthorizationContext);
    let navigate = useNavigate();
    
    const register = async (event) => {
        event.preventDefault()
        await axios.post(APIUrl + "/Auth/Register", {
            username: username,
            email: email,
            password: password
        })
        .then(function (response) {
            console.log(response)
            if(response.status == 200)
            {
                navigate("/login");
            }
        })
        .catch(function (error) {
            console.log(error)
        })
    }

    const navigateToLogin = () => {
        navigate("/login");
    }

    return (
        <div className="flex flex-col h-dvh justify-center bg-gradient-to-b from-slate-600 to-slate-400">
            <div className="flex flex-col h-2/3 w-1/4 bg-slate-600 mx-auto my-auto">
                <div className="flex flex-row h-1/5 w-full p-12">
                    <p className="text-4xl font-bold text-orange-400 ml-auto">Quiz</p>
                    <p className="text-4xl font-bold text-purple-400 mr-auto">Master</p>
                </div>
                <div className="flex flex-col h-3/4 w-full p-12">

                    <TextField className="bg-gray-100" id="outlined-basic" label="Username" variant="filled" onChange={(event) => {
                        setUsername(event.target.value)
                    }}/>

                     <div className="h-12"></div>

                    <TextField className="bg-gray-100" id="outlined-basic" label="EMail" variant="filled" onChange={(event) => {
                        setEmail(event.target.value)
                    }}/>

                    <div className="h-12"></div>
                    <TextField className="bg-gray-100" id="outlined-basic" label="Password" variant="filled" onChange={(event) => {
                        setPassword(event.target.value)
                    }}/>

                    <div className="h-12"></div>
                    <div className="flex flex-row w-full justify-center">
                        <Button className="w-1/4" variant="contained" onClick={register}>Register</Button>
                    </div>

                    <div className="h-12"></div>
                    <div className="flex flex-row h-fit">
                        <p className="text-white text-2xl mr-8">Already have an account?</p>
                        <Button className="w-1/4 mx-auto" variant="contained" onClick={navigateToLogin}>Login</Button>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default RegisterScreen