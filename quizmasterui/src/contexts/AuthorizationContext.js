import React from "react";
const AuthorizationContext = React.createContext({
    APIUrl: "http://127.0.0.1:5107/API",
    user: {
        id: -1,
        username: "",
        level: 0,
        experience: 0,
        expPercentage: 0,
        jwtToken: "",
        picture: ""
    },
    setUser: () => {},
});

export default AuthorizationContext;