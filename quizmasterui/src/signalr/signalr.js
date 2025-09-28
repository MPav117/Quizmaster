import * as signalR from "@microsoft/signalr"

var connection = null;

export const startConnection = async (jwtToken) => {
    if (connection == null)
    {
        await createConnection(jwtToken);
        return connection
    }
    else
    {
        await connection.stop();
        await createConnection(jwtToken);
        return connection
    }
}

export const stopConnection = async () => {
    if (connection != null && connection != undefined)
    {
        await connection.stop()
    }
}

export const connectionState = () => {
    if(connection == null || connection == undefined)
    {
        return null;
    }
    return connection.state
}

const createConnection = async (jwtToken) => {
    connection = new signalR.HubConnectionBuilder().withUrl("http://127.0.0.1:5107/gameHub", {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
        accessTokenFactory: () => jwtToken
    })
    .withAutomaticReconnect()
    .build()

    try
    {
        await connection.start();
        console.log("Connected to lobby.");
    }
    catch(err) {
        console.error("Failed to connect to lobby.")
    }

}

export const joinLobby = async (lobbyID, userID) => {
    try {
        await connection.invoke("AddToLobby", lobbyID, userID)
        console.log("Message sent - Join lobby!")
    }
    catch(err) {
        console.error(err)
    }
}

export const Ready = async (lobbyID, userID) => {
    try {
        await connection.invoke("Ready")
        console.log("Message sent - Ready!")
    }
    catch (err) {
        console.log(err)
    }
}

export const NotifyOfLobbyUpdate = async (lobbyID) => {
    try {
        await connection.invoke("NotifyOfLobbyUpdate", lobbyID)
        console.log("Message sent - Notify of Lobby Update!")
    }
    catch (err) {
        console.log(err)
    }
}

export const Unready = async (lobbyID, userID) => {
    try {
        await connection.invoke("Unready")
        console.log("Message sent - Not ready!")
    }
    catch (err) {
        console.log(err)
    }
}

export const StartQuiz = async (lobbyID, quizID) => {
    try {
        console.log(typeof(lobbyID))
        console.log(typeof(quizID))
        await connection.invoke("StartQuiz", lobbyID, quizID)
        console.log("Message sent - Quiz started!")
    }
    catch (err) {
        console.log(err)
    }
}

export const BuzzIn = async (lobbyID, questionID, userID) => {
    try {
        await connection.invoke("BuzzIn", lobbyID, questionID, userID)
        console.log("Message sent - Buzzed in!")
    }
    catch (err) {
        console.log(err)
    }
}

export const AnswerQuestion = async (lobbyID, questionID, userID, answer) => {
    try {
        await connection.invoke("AnswerQuestion", lobbyID, questionID, userID, answer)
        console.log("Message sent - Answered question!")
    }
    catch (err) {
        console.log(err)
    }
}

export const onUserChange = (callback) => {
    if (connection) {
        connection.on("onUserJoinLeaveLobby", callback)
        connection.on("onUserReadyUnready", callback)
    }
}

export const onUserReadyUnready = (callback) => {
    if (connection) {
        connection.on("onUserReadyUnready", callback)
    }
}

export const onLobbyUpdated = (callback) => {
    if (connection) {
        connection.on("onLobbyUpdated", callback)
    }
}

export const onQuizStart = (callback) => {
    if (connection) {
        connection.on("onQuizStart", callback)
    }
}

export const onGetNextQuestion = (callback) => {
    if (connection) {
        connection.on("onGetNextQuestion", questionID => {
            callback(questionID)
        })
    }
}

export const onBuzzRecieved = (callback) => {
    if (connection) {
        connection.on("onBuzzRecieved", userID => {
            callback(userID)
        })
    }
}

export const onIncorrectAnswerRecieved = (callback) => {
    if (connection) {
        connection.on("onIncorrectAnswerRecieved", userID => {
            callback(userID)
        })
    }
}

export const onCorrectAnswerRecieved = (callback) => {
    if (connection) {
        connection.on("onCorrectAnswerRecieved", userID => {
            callback(userID)
        })
    }
}

export const onTimeoutRecieved = (callback) => {
    if (connection) {
        connection.on("onTimeoutRecieved", miliseconds => {
            callback(miliseconds);
        })
    }
}

export const onTimedOut = (callback) => {
    if (connection) {
        connection.on("onTimedOut", callback);
    }
}

export const onQuizEnded = (callback) => {
    if (connection) {
        connection.on("onQuizEnded", callback)
    }
}