import React from "react";
import DisplayUserInQuiz from "../components/DisplayUserInQuiz";
import QuizQuestionHandler from "../components/QuizQuestionHandler";

export default function Quiz({lobby, quiz, sessions, quizQuestion, displayAnswerDisabled, inputDisabled, buzzDisabled, handleBuzz, handleInput, timeLeft})
{
    return (
        <div className="flex flex-col mx-auto my-auto h-[95%] w-[95%] bg-slate-500">
            <div className="flex flex-row h-[60%] w-full">
                {/* small left box, displays players 1 and 3 */}
                <div className="flex flex-col mx-auto my-4 h-full w-[20%] bg-slate-600">
                    {sessions[0] && 
                        <DisplayUserInQuiz displayAnswerDisabled={displayAnswerDisabled} session={sessions[0]} index={0}></DisplayUserInQuiz>
                    }
                    {sessions[2] && 
                        <DisplayUserInQuiz displayAnswerDisabled={displayAnswerDisabled} session={sessions[2]} index={2}></DisplayUserInQuiz>
                    }
                </div>
                {/* big box, displays quiz question/potentially images relating to quiz */}
                <div className="flex flex-col mx-4 my-4 h-full w-[55%] bg-slate-600">
                    {quizQuestion && <p className="ml-auto mb-auto mr-16 mt-16 text-white text-3xl">{timeLeft}</p>}
                    {quizQuestion && <p className="mx-auto mb-auto text-white text-3xl">{quizQuestion.question}</p>}
                    {quizQuestion && <p className="mx-auto mb-auto text-white text-3xl">{quizQuestion.pointValue.toString() + " points"}</p>}
                    {quizQuestion && <p className="mx-auto mb-auto text-white text-3xl">{displayAnswerDisabled ? "" : quizQuestion.answer}</p>}
                </div>
                 {/* small left box, displays players 2 and 4 */}
                <div className="flex flex-col mx-auto my-4 h-full w-[20%] bg-slate-600">
                    {sessions[1] && 
                        <DisplayUserInQuiz displayAnswerDisabled={displayAnswerDisabled} session={sessions[1]} index={1}></DisplayUserInQuiz>
                    }
                    {sessions[3] && 
                        <DisplayUserInQuiz displayAnswerDisabled={displayAnswerDisabled} session={sessions[3]} index={3}></DisplayUserInQuiz>
                    }
                </div>
            </div>
            {/* bottom box, for inputs */}
            <div className="flex flex-row flex-wrap mx-auto my-auto h-[30%] w-[95%]">
                {/* buttons */}
                <QuizQuestionHandler quizQuestion={quizQuestion} inputDisabled={inputDisabled} buzzDisabled={buzzDisabled} handleInput={handleInput} handleBuzz={handleBuzz}></QuizQuestionHandler>
            </div>
        </div>
    )
}