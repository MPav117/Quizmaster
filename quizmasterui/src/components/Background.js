import React from "react";

export function Background ({children, style, onChange})
{
    return (
        <div className="flex flex-col overflow-clip min-h-[45rem] min-w-[80rem] h-dvh justify-center bg-gradient-to-b from-slate-600 to-slate-400">
            {children}
        </div>
    )
}