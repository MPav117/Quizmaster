import React from "react"

export default function DisplayUserInQuiz({session, index, displayAnswerDisabled})
{
    const colorOuter = ["bg-blue-500", "bg-green-500", "bg-yellow-500", "bg-red-500"]
    const colorInner = ["bg-blue-600", "bg-green-600", "bg-yellow-600", "bg-red-600"]

    return (
        <div className={"flex flex-col mx-auto my-auto w-[90%] h-[40%] " + colorOuter[index]}>
            <div className={"flex flex-row mx-auto my-auto w-full h-[45%] " + colorInner[index]}>
                <div className={"flex mx-auto my-auto w-[25%] h-[95%] " + colorOuter[index] }>

                </div>
                <div className={"flex mx-auto my-auto w-[70%] h-[95%] " + colorOuter[index]}>
                    <p className="mx-auto my-auto text-xl text-white">{session.user.username}</p>
                </div>
            </div>
            <div className={"flex flex-row mx-auto my-auto w-full h-[45%] " + colorInner[index]}>
                <p className="mx-auto my-auto text-2xl text-white">{displayAnswerDisabled === false ? (session.answer ? "Answer: " + session.answer : " " ) : session.points + " points"}</p>
            </div>
        </div>
    )
}