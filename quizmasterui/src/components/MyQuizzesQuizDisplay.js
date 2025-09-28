import { GenericButton } from "./Buttons";

export default function MyQuizzesQuizDisplay({quiz, onEditClick, onDeleteClick}) 
{
    return (
        <div className="mt-16 mx-auto flex-col h-[30%] w-[20%] flex overflow-hidden text-ellipsis bg-orange-400 shadow-orange-600 shadow-md">
            <p className="text-xl text-white mx-auto my-2">{quiz.name}</p>
            <p className="text-white mx-auto my-2 line-clamp-3 w-[90%]">{quiz.description}</p>
            <p className="text-white mx-auto mt-auto mb-2">{quiz.questionCount}</p>
            <div className="flex flex-row w-full mt-2 mb-4">
                <GenericButton className="ml-4 mr-auto w-[25%] h-fit bg-green-500 shadow-green-700 shadow-md" onClick={onEditClick} text={"Edit"}></GenericButton>
                <GenericButton className="mr-4 ml-auto w-[25%] h-fit bg-red-500 shadow-red-700 shadow-md" onClick={onDeleteClick} text={"Delete"}></GenericButton>
            </div>
        </div>
    )
}