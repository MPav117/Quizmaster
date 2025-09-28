import React, { useContext, useEffect, useState } from "react";
import axios from "axios";
import { Background } from "../components/Background";
import AuthorizationContext from "../contexts/AuthorizationContext";
import { GenericButton } from "../components/Buttons";
import { useNavigate } from "react-router-dom";

export default function CreateQuiz({quiz, setEditQuiz})
{  
    const {APIUrl, user} = useContext(AuthorizationContext);
   
    const [quizID, setQuizID] = useState(-1);
    const [quizName, setQuizName] = useState("My Quiz");
    const [quizDescription, setQuizDescription] = useState("This is the default quiz description.");
    
    const [questionList, setQuestionList] = useState([]);
    
    const [editQuestionIndex, setEditQuestionIndex] = useState(-1);

    const [questionText, setQuestionText] = useState("What is the capital of Germany?");
    const [questionAnswer, setQuestionAnswer] = useState("Berlin");
    const [questionType, setQuestionType] = useState("ChooseOutOf");
    
    const [offeredAnswer1, setOfferedAnswer1] = useState("");
    const [offeredAnswer2, setOfferedAnswer2] = useState("");
    const [offeredAnswer3, setOfferedAnswer3] = useState("");
    const [offeredAnswer4, setOfferedAnswer4] = useState("");

    const [errorLabel, setErrorLabel] = useState("");
    const [successLabel, setSuccessLabel] = useState("");

    const [pointValue, setPointValue] = useState("10");

    const navigate = useNavigate();

    const questionTypes = ["ChooseOutOf", "InputOwnAnswer", "ClosestNumber", "PriceIsRight"]

    useEffect(() => {
        handleChangeQuiz();
    }, [quiz])

    const handleChangeQuiz = async () => {

        resetScreen()

        if(quiz != null)
        {
            setQuizID(quiz.id)
            setQuizName(quiz.name)
            setQuizDescription(quiz.description)
            
            const quizPath = "/Quiz/ReadQuestionsOfQuiz/"
            console.log(APIUrl + quizPath + quiz.id)
            await axios.get(APIUrl + quizPath + quiz.id)
            .then((result) => {
                console.log(result.data)
                setQuestionList(result.data)
            })
            .catch(err => {
                console.log(err)
            })
        }
    }

    const resetScreen = () => {
        setQuizID(-1);
        setQuizName("My Quiz")
        setQuizDescription("This is the default quiz description.")
            
        setQuestionList([])

        setEditQuestionIndex(-1);

        setQuestionText("What is the capital of Germany?")
        setQuestionAnswer("Berlin")
        setQuestionType("ChooseOutOf")
            
        setOfferedAnswer1("")
        setOfferedAnswer2("")
        setOfferedAnswer3("")
        setOfferedAnswer4("")
            
        setPointValue("10")

        setErrorLabel("")
        setSuccessLabel("");
    }

    const handleSetEditQuestion = (index) => {
        var localQuestion = questionList[index]
        setEditQuestionIndex(index)
        setQuestionText(localQuestion.question)
        setQuestionAnswer(localQuestion.answer)
        setQuestionType(questionTypes[localQuestion.questionType])
        setPointValue(localQuestion.pointValue)
        
        if(localQuestion.questionType == 0)
        {
            setOfferedAnswer1(localQuestion.offeredAnswer1)
            setOfferedAnswer2(localQuestion.offeredAnswer2)
            setOfferedAnswer3(localQuestion.offeredAnswer3)
            setOfferedAnswer4(localQuestion.offeredAnswer4)
        }
    }

    const validation = () => {
        if(questionType == "ChooseOutOf")
        {
            if((questionAnswer != offeredAnswer1) && (questionAnswer != offeredAnswer2) && (questionAnswer != offeredAnswer3) && (questionAnswer != offeredAnswer4))
            {
                setSuccessLabel("")
                setErrorLabel("Answer not among offered answers!");
                return false;
            }
        }
        
        if(questionType == "ClosestNumber" || questionType == "PriceIsRight")
        {
            console.log(questionAnswer)
            if(isNaN(questionAnswer))
            {
                setSuccessLabel("")
                setErrorLabel("Answer is not a number!");
                return false;
            }
        }

        if(isNaN(pointValue))
        {
            setSuccessLabel("")
            setErrorLabel("Point value invalid!");
            return false;
        }
    }

    const handleEditQuestion = () => {

        if (validation() == false)
        {
            return;
        }

        var editedQuestion = questionList[editQuestionIndex]
        var localQuestionList = questionList.slice()

        localQuestionList.splice(editQuestionIndex, 1)

        editedQuestion.question = questionText;
        editedQuestion.answer = questionAnswer;
        editedQuestion.pointValue = pointValue;
        editedQuestion.questionType = questionType;

        if(questionType == "ChooseOutOf")
        {
            editedQuestion.offeredAnswer1 = offeredAnswer1;
            editedQuestion.offeredAnswer2 = offeredAnswer2
            editedQuestion.offeredAnswer3 = offeredAnswer3
            editedQuestion.offeredAnswer4 = offeredAnswer4
        }

        var list = []
        list = list.concat(questionList)
        list.push(editedQuestion)

        setQuestionList(list);
        setEditQuestionIndex(-1);
    }

    const handleAddNewQuestion = () => {
        var newQuestion = {
            quizID: null,
            question: questionText,
            answer: questionAnswer,
            questionType: questionTypes.indexOf(questionType),
            image: null,
            pointValue: parseInt(pointValue)
        }

        if(quiz != null)
        {
            newQuestion.quizID = quiz.id;
        }

        if (validation() == false)
        {
            return;
        }
        
        if(questionType == "ChooseOutOf")
        {
            newQuestion.offeredAnswer1 = offeredAnswer1;
            newQuestion.offeredAnswer2 = offeredAnswer2
            newQuestion.offeredAnswer3 = offeredAnswer3
            newQuestion.offeredAnswer4 = offeredAnswer4
        }

        var list = []
        list = list.concat(questionList)
        list.push(newQuestion)

        setQuestionList(list)
        setErrorLabel("");
        setSuccessLabel("Successfully added question!");
    }

    const handleSubmitQuestion = () => {
        if(editQuestionIndex == -1)
        {
            handleAddNewQuestion();
        }
        else
        {
            handleEditQuestion();
        }
    }

    const removeQuestion = (index) => {
        console.log(index)
        var list = []
        list = list.concat(questionList)
        list.splice(index, 1)
        setQuestionList(list)
        if(editQuestionIndex == index)
        {
            setEditQuestionIndex(-1)
        }

    }

    const handleChangePointValue = (e) => {
        console.log(e)
        try {
            if(isNaN(e) == false)
            {
                setPointValue(e);
            }
        }
        catch(err) {
            console.log(err);
        }
    }

    const createQuiz = async (e) => {
        e.preventDefault()
        const quizPath = "/Quiz/CreateQuiz"
        const questionPath = "/Quiz/CreateQuizQuestions"
        
        var newQuiz = {
            name: quizName,
            questionCount: questionList.length,
            description: quizDescription,
            creatorID: user.id
        };

        console.log(newQuiz)
        
        await axios.post(APIUrl + quizPath, newQuiz)
        .then(async response => {
            console.log(response)
            var sendQuestionList = questionList.slice()
            for(let i = 0; i < sendQuestionList.length; i++){
                sendQuestionList[i].quizID = response.data.id
            }
            await axios.post(APIUrl + questionPath, sendQuestionList)
            .then(async response => {
                setSuccessLabel("Successfully created quiz!");
                console.log(response)
                handleChangeQuiz();
            })
            .catch(err => {
                setErrorLabel("Error when adding quiz questions...");
                console.log(err)
            })
        })
        .catch(err => {
            setErrorLabel("Error when creating quiz...");
            console.log(err)
        })
    }

    const updateQuiz = async (e) => {
        e.preventDefault()
        const updatePath = "/Quiz/UpdateQuizQuestions/"
        const questionPath = "/Quiz/CreateQuizQuestions"

        console.log(questionList)

        var updateList = []
        var createList = []

        questionList.forEach(question => {
            console.log(question.id)
            if(question.id !== null && question.id !== undefined)
            {
                updateList.push(question)
            }
            else
            {
                createList.push(question)
            }
        });

        console.log(createList)
        console.log(updateList)

        await axios.post(APIUrl + updatePath, updateList)
        .then(async response => {
            console.log(response)
            setSuccessLabel("Successfully updated old questions!");
            
            await axios.post(APIUrl + questionPath, createList)
            .then(response => {
                setSuccessLabel("Successfully created new questions!");
                console.log(response)
            })
            .catch(err => {
                setErrorLabel("Error when adding quiz questions...");
                console.log(err)
            })
        })
        .catch(err => {
            console.log(err)
            setErrorLabel("Failed to update quiz!");
        })
    }

    const back = () => {
        setEditQuiz(false)
    }
    return (
        <>
                <div className="flex flex-row w-[90%] h-[90%] bg-slate-700 mx-auto my-auto" >
                    <div className="flex flex-col rounded-lg h-[90%] w-3/5 my-auto mx-12 bg-slate-600">
                        <GenericButton onClick={back} text="Cancel" className="w-fit p-2 h-10 ml-4 mr-auto my-4 rounded-md bg-purple-500 shadow-purple-700 hover:bg-purple-700 hover:shadow-purple-800 shadow-md self-end"></GenericButton>
                        <div className="flex flex-row w-full">
                            <div className="flex flex-col w-[50%]">
                                <div className= "flex flex-row rounded-lg w-[90%] my-2 mx-4">
                                    <label className="text-white">
                                        Quiz name:
                                        <input className="w-full text-black" type="text" value={quizName} onChange={(e) => setQuizName(e.target.value)}></input>
                                    </label>
                                </div>
                                <div className= "flex flex-row rounded-lg w-[90%] my-2 mx-4">
                                    <label className="text-white">
                                        Quiz description:
                                        <input className="w-full text-black" value={quizDescription} onChange={e => setQuizDescription(e.target.value)}></input>
                                    </label>
                                </div>
                            </div>
                            <div className="flex flex-col w-[50%]">
                                {quizID == -1 &&<GenericButton onClick={createQuiz} text="Create Quiz" className="w-[80%] h-[80%] lg:text-3xl mx-auto my-auto rounded-md bg-green-500 shadow-green-700 hover:bg-green-700 hover:shadow-green-800 shadow-md self-end"></GenericButton>}
                                {quizID != -1 &&<GenericButton onClick={updateQuiz} text="Update Quiz" className="w-[80%] h-[80%] lg:text-3xl mx-auto my-auto rounded-md bg-green-500 shadow-green-700 hover:bg-green-700 hover:shadow-green-800 shadow-md self-end"></GenericButton>}
                            </div>
                        </div>
                        <div className="flex flex-col overflow-y-scroll rounded-lg h-[70%] w-[90%] my-4 mx-4 bg-slate-500">
                            <div className= "flex rounded-lg w-[90%] my-2 mx-4">
                                <label className="text-white w-[70%]">
                                    Question:
                                    <input className="w-full text-black" value={questionText} onChange={e => setQuestionText(e.target.value)}></input>
                                </label>
                            </div>
                            <div className= "flex rounded-lg w-[90%] my-2 mx-4">
                                <label className="text-white w-[70%]">
                                    Answer:
                                    <input className="w-full text-black" value={questionAnswer} onChange={e => setQuestionAnswer(e.target.value)}></input>
                                </label>
                            </div> 
                            <div className= "flex rounded-lg w-[90%] my-2 mx-4">
                                <label className="text-white">
                                    Type of question:
                                    <select className="w-[70%] text-black" value = {questionType} onChange = {e => setQuestionType(e.target.value)}>
                                        {
                                            questionTypes.map(type => (
                                                <option value={type}>{type}</option>
                                            ))
                                        }
                                    </select>
                                </label>
                            </div> 
                            <div className= "flex rounded-lg w-[90%] my-4 mx-4">
                                <label className="text-white">
                                    Point value:
                                    <input className="mx-2 text-black" value={pointValue} onChange={e => handleChangePointValue(e.target.value)}></input>
                                </label>
                            </div>
                            {(questionType == "ChooseOutOf") && 
                                <div className= "flex flex-row flex-wrap"> 
                                    <div className= "flex rounded-lg w-[45%] my-4 mx-4">
                                         <label className="text-white w-full">
                                            Option 1:
                                            <input className="mx-2 text-black" value={offeredAnswer1} onChange={e => setOfferedAnswer1(e.target.value)}></input>
                                        </label>
                                    </div>
                                    <div className= "flex rounded-lg w-[45%] my-4 mx-4">
                                         <label className="text-white w-full">
                                            Option 2:
                                            <input className="mx-2 text-black" value={offeredAnswer2} onChange={e => setOfferedAnswer2(e.target.value)}></input>
                                        </label>
                                    </div>
                                    <div className= "flex rounded-lg w-[45%] my-4 mx-4">
                                         <label className="text-white w-full">
                                            Option 3:
                                            <input className="mx-2 text-black" value={offeredAnswer3} onChange={e => setOfferedAnswer3(e.target.value)}></input>
                                        </label>
                                    </div>
                                    <div className= "flex rounded-lg w-[45%] my-4 mx-4">
                                         <label className="text-white w-full">
                                            Option 4:
                                            <input className="mx-2 text-black" value={offeredAnswer4} onChange={e => setOfferedAnswer4(e.target.value)}></input>
                                        </label>
                                    </div>
                                </div>
                            }
                            <p className="text-red-500 text-2xl flex mx-auto">{errorLabel}</p>
                            <p className="text-green-500 text-2xl flex mx-auto">{successLabel}</p>
                        </div>
                        <GenericButton text="Add Question" className="w-32 h-12 mt-auto mb-4 mr-4 rounded-md bg-green-500 shadow-green-700 hover:bg-green-700 hover:shadow-green-800 shadow-md self-end" onClick={handleSubmitQuestion}></GenericButton>
                    </div>

                    <div className="flex flex-col flex-wrap rounded-lg overflow-auto h-[90%] w-1/3 my-auto mx-12 bg-slate-500">
                        <ul>
                        {
                            questionList.map((question, index) => (
                                <li key={index}>
                                    <div className="flex flex-col justify-center w-[95%] mx-auto my-2 shadow-sm bg-slate-300 shadow-slate-400 border-2 border-slate-800">
                                        <div className = "flex flex-row w-full">
                                            <GenericButton text="Edit" onClick={() => handleSetEditQuestion(index)} className={"w-fit h-fit shadow-md ml-2 mr-auto my-2 p-2 bg-purple-500 shadow-purple-700 hover:bg-purple-600 hover:shadow-purple-800"}></GenericButton>
                                            <p className="mt-4 mx-auto text-2xl">{"Question " + (index+1)}</p>
                                            <GenericButton text="Delete" onClick={() => removeQuestion(index)} className={"w-fit h-fit shadow-md mr-2 ml-auto my-2 p-2 bg-red-500 shadow-red-700 hover:bg-red-600 hover:shadow-red-800"}></GenericButton>
                                        </div>
                                        <p className="mx-auto">{question.question}</p>
                                        <p className="mx-auto">{question.answer}</p>
                                        <p className="mx-auto">{question.pointValue + " points"}</p>
                                        {question.questionType == 0 && 
                                            <div className = "flex flex-col mt-4">  
                                                <div className = "flex flex-row text-ellipsis justify-around"> 
                                                    <p className="w-[40%]">{question.offeredAnswer1}</p>
                                                    <p className="w-[40%]">{question.offeredAnswer2}</p>
                                                </div>
                                                <div className="flex flex-row justify-around text-ellipsis">
                                                    <p className="w-[40%]">{question.offeredAnswer3}</p>
                                                    <p className="w-[40%]">{question.offeredAnswer4}</p>
                                                </div>
                                            </div>
                                        }
                                    </div>
                                </li>
                            ))
                        }
                        </ul>
                    </div>
                </div>
        </>
    )
}