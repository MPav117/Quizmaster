import React from "react"
import { GenericButton } from "./Buttons"
import { Navigate, useNavigate } from "react-router-dom"

export function QuizListItem ({quiz, onClick})
{
    return(
        <div className="flex flex-row mx-8 my-4 h-36 w-[90%] rounded-lg bg-blue-400">
            <div className="flex flex-auto my-auto mx-4 h-2/3 bg-blue-300 rounded-xl">
                <div className="flex flex-auto mx-2 h-full w-1/3 my-auto">
                    <p className="my-auto text-xl">{quiz.name}</p>
                </div>
                <div className="flex flex-auto mx-2 h-full w-1/3 my-auto">
                    <p className="my-auto text-xl">{quiz.description}</p>
                </div>
                <div className="flex flex-auto flex-col mx-2 h-full w-1/3 my-auto">
                    <p className="mt-auto text-xl">Question count:</p>
                    <p className="mb-auto text-xl">{quiz.questionCount}</p>
                </div>
            </div>
            <GenericButton onClick={onClick} text="Select Quiz" className = "text-2xl h-4/5 w-1/5 my-auto mx-2 rounded-md shadow-md bg-orange-400 shadow-orange-600 hover:bg-orange-500 hover:shadow-orange-700 hover:scale-105 transition duration-10" />
        </div>
    )
}