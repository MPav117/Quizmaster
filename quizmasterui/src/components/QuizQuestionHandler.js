import React, { useState, useEffect } from "react";
import { GenericButton } from "./Buttons";
export default function QuizQuestionHandler({quizQuestion, inputDisabled, buzzDisabled, handleInput, handleBuzz}) {

    return (
        <div className="flex flex-row h-full w-full ">
            {quizQuestion && 
            <>
            {(quizQuestion.questionType == 0) && <ChooseOutOf quizQuestion={quizQuestion} inputDisabled={inputDisabled} buzzDisabled={buzzDisabled} handleInput={handleInput} handleBuzz={handleBuzz} />}
            {(quizQuestion.questionType == 1) && <InputOwnAnswer quizQuestion={quizQuestion} inputDisabled={inputDisabled} buzzDisabled={buzzDisabled} handleInput={handleInput} handleBuzz={handleBuzz} />}
            {(quizQuestion.questionType == 2 || quizQuestion.questionType == 3) && <ClosestNumber quizQuestion={quizQuestion} inputDisabled={inputDisabled} buzzDisabled={buzzDisabled} handleInput={handleInput} handleBuzz={handleBuzz} />}
            </>
        }
        </div>
    )
}

function ChooseOutOf({quizQuestion, inputDisabled, buzzDisabled, handleInput, handleBuzz}) {
    return (
        <div className="flex flex-row h-full w-full">
            <div className="flex flex-col h-full w-[40%]">
                    <div className="flex mx-auto my-auto h-[40%] w-full">
                        <GenericButton onClick={() => handleInput(quizQuestion.offeredAnswer1)} disabled={inputDisabled} className={(inputDisabled ? "bg-gray-600 shadow-gray-800" : "bg-red-600 shadow-red-800 hover:bg-red-700 hover:shadow-red-900 ") + "flex w-full h-full shadow-md rounded-2xl text-2xl"} text={quizQuestion.offeredAnswer1}></GenericButton>
                    </div>
                    <div className="flex mx-auto my-auto h-[40%] w-full">
                        <GenericButton onClick={() => handleInput(quizQuestion.offeredAnswer3)} disabled={inputDisabled} className={(inputDisabled ? "bg-gray-600 shadow-gray-800" : "bg-blue-600 shadow-blue-800 hover:bg-blue-700 hover:shadow-blue-900 ") + "flex w-full h-full shadow-md rounded-2xl text-2xl"} text={quizQuestion.offeredAnswer3}></GenericButton>
                    </div>
                </div>

                <div className="flex flex-col h-full w-[20%]">
                    <GenericButton onClick={handleBuzz} disabled={buzzDisabled} className={(buzzDisabled ? "bg-gray-600 shadow-gray-800" : "bg-red-600 shadow-red-800 hover:bg-red-700 hover:shadow-red-900 ") + "my-auto mx-auto flex w-[80%] h-[80%] shadow-lg rounded-full text-3xl"} text={"Buzz"}></GenericButton>
                </div>
                
                <div className="flex flex-col h-full w-[40%]">
                    <div className="flex mx-auto my-auto h-[40%] w-full">
                        <GenericButton onClick={() => handleInput(quizQuestion.offeredAnswer2)} disabled={inputDisabled} className={(inputDisabled ? "bg-gray-600 shadow-gray-800" : "bg-green-600 shadow-green-800 hover:bg-green-700 hover:shadow-green-900 ") + "flex w-full h-full shadow-md rounded-2xl text-2xl"} text={quizQuestion.offeredAnswer2}></GenericButton>
                    </div>
                
                    <div className="flex mx-auto my-auto h-[40%] w-full">
                        <GenericButton onClick={() => handleInput(quizQuestion.offeredAnswer4)} disabled={inputDisabled} className={(inputDisabled ? "bg-gray-600 shadow-gray-800" : "bg-yellow-600 shadow-yellow-800 hover:bg-yellow-700 hover:shadow-yellow-900 ") + "flex w-full h-full shadow-md rounded-2xl text-2xl"} text={quizQuestion.offeredAnswer4}></GenericButton>
                    </div>
                </div>
        </div>
    )
}

function InputOwnAnswer({quizQuestion, inputDisabled, buzzDisabled, handleInput, handleBuzz}) {
    const [input, setInput] = useState("")

    useEffect(() => {
        setInput("")
    }, [quizQuestion])

    return (
        <div className="flex flex-row h-full w-full">

        {(inputDisabled == true) &&
            <>
                <div className="flex flex-col h-full w-[40%]">

                </div>
                <div className="flex flex-col h-full w-[20%]">
                    <GenericButton onClick={handleBuzz} disabled={buzzDisabled} className={(buzzDisabled ? "bg-gray-600 shadow-gray-800" : "bg-red-600 shadow-red-800 hover:bg-red-700 hover:shadow-red-900 ") + "my-auto mx-auto flex w-[80%] h-[80%] shadow-lg rounded-full text-3xl"} text={"Buzz"}></GenericButton>
                </div>
                <div className="flex flex-col h-full w-[40%]">

                </div>
            </>
        }

        {
            (inputDisabled == false) &&
            <>
                <div className="flex flex-col h-full w-[80%]">
                    <input className="mx-auto my-auto h-[30%] w-[80%] text-2xl text-center text-black" value={input} onChange={(e) => setInput(e.target.value)}></input>
                </div>
                <div className="flex flex-col h-full w-[20%]">
                    <GenericButton onClick={() => handleInput(input)} disabled={inputDisabled} className={"bg-green-600 shadow-green-800 hover:bg-green-700 hover:shadow-green-900 flex w-full h-full shadow-md rounded-2xl text-2xl"} text={"Submit"}></GenericButton>
                </div>
            </>
        }
        </div>
    )
}

function ClosestNumber({quizQuestion, inputDisabled, buzzDisabled, handleInput, handleBuzz}) {
    const [input, setInput] = useState("")
    const [error, setError] = useState("")

    const handleInputNumber = (input) => {
        if(Number.isNaN(input))
        {
            setError("Input is not a number!");
            return;
        }
        else
        {
            handleInput(input);
        }
    }

    useEffect(() => {
        setInput("")
    }, [quizQuestion])

    return (
        <div className="flex flex-row h-full w-full">
            <div className="flex flex-col h-full w-[80%]">
                <input className="mx-auto my-auto h-[30%] w-[80%] text-2xl text-center text-black" disabled={buzzDisabled} value={input} onChange={(e) => setInput(e.target.value)}></input>
            </div>
            <div className="flex flex-col h-full w-[20%]">
                <GenericButton onClick={() => handleInputNumber(input)} disabled={buzzDisabled} className={(buzzDisabled ? "bg-gray-600 shadow-gray-800" : "bg-green-600 shadow-green-800 hover:bg-green-700 hover:shadow-green-900") +  " flex w-full h-full shadow-md rounded-2xl text-2xl"} text={"Submit"}></GenericButton>
            </div>
            <p className="flex text-red text-3xl"></p>
        </div>
    )
}

