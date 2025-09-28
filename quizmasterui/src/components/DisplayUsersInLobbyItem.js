import React from "react";

export default function DisplayUsersInLobbyItem({session, index})
{
    const colorOuter = ["bg-blue-500", "bg-green-500", "bg-yellow-500", "bg-red-500"]
    const colorInner = ["bg-blue-600", "bg-green-600", "bg-yellow-600", "bg-red-600"]

    return (
        <div className={"flex flex-row w-[75%] h-36 mx-auto my-4 " + colorOuter[index]}>
            <div className={"mx-4 my-auto h-[75%] w-[15%] " + colorInner[index]}>
                {/* TODO: Put the profile picture here :> */}
            </div>
            <div className="flex flex-col flex-auto">
                <p className="text-white mx-2 mt-auto text-3xl">{session.user.username}</p>
                <p className="text-white mx-2 mb-auto text-2xl">{`Level: ${session.user.level}`}</p>
            </div>
            <p className="text-white mx-4 my-auto text-2xl">{session.ready ? "Ready" : "Not Ready"}</p>
        </div>
    )
}