import React from "react";

export default function TextInput({value, onChange, className, children})
{
    return (
        <div className={className != null ? className : ""}>
            <label className="text-white">
                {children != null ? children : ""}
                <input className="w-full text-black" value={value} onChange={onChange}></input>
            </label>
        </div>
    )
}