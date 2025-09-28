import { Background } from "../components/Background";
import React, { useState, useContext, useEffect } from "react";
import AuthorizationContext from "../contexts/AuthorizationContext";
import MyQuizzesQuizDisplay from "../components/MyQuizzesQuizDisplay";
import axios from "axios";
import CreateQuiz from "./CreateQuiz";
import { GenericButton } from "../components/Buttons";

export default function MyQuizzes()
{
    const {user, APIUrl} = useContext(AuthorizationContext)

    const [quizzes, setQuizzes] = useState([])
    const [selectedQuiz, setSelectedQuiz] = useState(null);
    const [editQuiz, setEditQuiz] = useState(false);

    useEffect(() => {
        handleGetMyQuizzes();
    }, [])

    const handleGetMyQuizzes = async () => {
        const getPath = "/Quiz/GetAllQuizzesCreatedByUser/"

        if(user.id != -1)
        {
            await axios.get(APIUrl + getPath + user.id)
            .then(result => {
                console.log(result)
                setQuizzes(result.data)
            })
            .catch(error => {
                console.log(error)
            })
        }

    }

    const handleUpdateQuiz = (quiz) => {
        setSelectedQuiz(quiz)
        setEditQuiz(true)
    }

    const handleCreateQuiz = () => {
        setSelectedQuiz(null)
        setEditQuiz(true)
    }

    const handleDeleteQuiz = async (id) => {
        console.log("Test")
        const deletePath = "/Quiz/DeleteQuiz/"
        await axios.delete(APIUrl + deletePath + id)
        .then(response => {
            console.log(response)
            handleGetMyQuizzes()
        })
        .catch(err => {
            console.log(err)
        })
    }

    return (
        <Background>
            {editQuiz == false && <div className="mx-auto my-auto flex flex-col h-[90%] w-[90%] bg-slate-700">
                <div className="mx-auto mt-4 mb-auto flex flew-row flex-wrap h-[80%] w-[95%] overflow-y-scroll bg-slate-600">
                    {quizzes && quizzes.map((quiz, index) => (
                    <MyQuizzesQuizDisplay quiz={quiz} onEditClick={() => handleUpdateQuiz(quiz)} onDeleteClick={() => handleDeleteQuiz(quiz.id)}> </MyQuizzesQuizDisplay>
                    ))}
                </div>
                <GenericButton className="text-white mx-auto my-auto text-2xl text-clip bg-green-500 shadow-green-700 hover:bg-green-600 hover:shadow-green-800 shadow-md w-[60%] h-[10%]" text="Create New Quiz" onClick={() => handleCreateQuiz()}></GenericButton>
            </div>}
            {editQuiz == true && <CreateQuiz quiz={selectedQuiz} setEditQuiz={setEditQuiz}></CreateQuiz>}
        </Background>
    )
}