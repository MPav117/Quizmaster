import React from "react"
import { Background } from "../components/Background"
import { GenericButton } from "../components/Buttons"

export default function Endcard({sessions, handleReturnToLobby})
{
    const colorOuter = ["bg-blue-500", "bg-green-500", "bg-yellow-500", "bg-red-500"]
    const colorInner = ["bg-blue-600", "bg-green-600", "bg-yellow-600", "bg-red-600"]

    const getPosition = (session) => {
        var size = 0;
        sessions.forEach(element => {
            if(element.points > session.points)
            {
                size = size + 1;
            }
        });
        return size;
    } 

    return (
            <div className="flex flex-col content-evenly w-[90%] h-[90%] bg-slate-700 mx-auto my-auto">
                <div>
                    <GenericButton className = "flex mr-4 ml-auto mt-4 px-2 py-2 w-fit bg-purple-500 shadow-purple-700 shadow-md hover:bg-purple-600 hover:shadow-purple-800 text-3xl" text="Return to Lobby" onClick={handleReturnToLobby}></GenericButton>
                </div>

                <div className="flex flex-row h-full w-full">
                    {
                    sessions.map((value, index) => {
                        var height = 90 - getPosition(value) * 10;
                        var heightString = ` h-[${height.toString()}%] `
                        return (
                        <div className={colorOuter[index] + heightString + "flex flex-col w-[20%] mt-auto mx-auto "}>
                            <div className={colorInner[index] + " flex w-24 h-24 mx-auto mt-10 mb-2"}>
                                <p className="text-4xl my-auto mx-auto text-white">{"#" + (getPosition(value)+1).toString()}</p>  
                            </div>
                            <p className="text-3xl my-4 mx-auto w-[95%] max-h-[50%] text-center text-balance text-white">{value.user.username}</p>
                            <p className="text-3xl my-4 mx-auto text-white">{value.points.toString()}</p>
                        </div>
                        );
                    })
                }   
                </div> 
            </div>
    )
}