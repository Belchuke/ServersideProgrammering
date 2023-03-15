import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom'
import { useMutation } from '@apollo/client';
import { REGISTER_USER } from '../api/UserEndpoints';


const RegisterView = () => {
    const navigate = useNavigate();
    const [userState, setUserState] = useState({
        userName: '',
        password: '',
    });
    const [apiResponseState, setApiResponseState] = useState({
        message: '',
        isSuccessful: false
    });
    const [registerMutation] = useMutation(REGISTER_USER);

    //const [mutationFunction, { loading, error, data }] = useMutation(registerUser("test", "hej"))
    //   const {loading, error, data} = useMutation(registerUser("test", "hej"));

    const handleRegister = () => {
        registerMutation({
            variables: {
                userName: userState.userName,
                password: userState.password
            }
        })
        .then(res => {
            if (res.data.registerUser.isSuccessful)
                navigate('/two-factor-auth', { state: { username: userState.userName }});
            else
                setApiResponseState({
                   isSuccessful: res.data.registerUser.isSuccessful,
                   message:      res.data.registerUser.message });
        })
        .catch(err => console.log(err));
    };

    return (
        <div>
            <p>Username (Email):</p>
            <input 
                value={userState.userName}
                onChange={e => setUserState(prev => { return { ...prev, userName: e.target.value } })}
                placeholder='Username'
                type="text"
            />
            <p>Password:</p>
            <input
                value={userState.password}
                onChange={e => setUserState(prev => { return { ...prev, password: e.target.value } })}
                placeholder='Password'
                type="password"
            />
            {!apiResponseState.isSuccessful ? <p>{apiResponseState.message}</p> : false}
            <button onClick={handleRegister}>Register</button>
        </div>
    );
}

//const handleRegister = async () => {
//    const { userName, password } = this.state;
//
//    try {
//      const user = await this.props.signupUserMutation({variables: {userName, password }})
//      localStorage.setItem('graphcoolToken', user.data.signupUser.token)
//      this.props.history.replace('/')
//    } catch (e) {
//      console.error(`An error occured: `, e)
//      this.props.history.replace('/')
//    }
//}

export default RegisterView;