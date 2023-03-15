import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom'
import { useMutation } from '@apollo/client';
import { SIGN_IN } from '../api/UserEndpoints';


const SignInView = () => {
    const navigate = useNavigate();
    const [userState, setUserState] = useState({
        username: '',
        password: '',
    });
    const [apiResponseState, setApiResponseState] = useState({
        message: '',
        isSuccessful: false
    });
    const [signInMutation] = useMutation(SIGN_IN);

    const handleSignIn = () => {
        signInMutation({
            variables: {
                username: userState.username,
                password: userState.password
            }
        })
        .then(res => {
            if (res.data.signIn.isSuccessful)
                navigate('two-factor-auth', { state: { username: userState.username }});
            else
                setApiResponseState({
                   isSuccessful: res.data.signIn.isSuccessful,
                   message:      res.data.signIn.message });
        })
        .catch(err => console.log(err));
    }

    return (
        <div>
            <p>Username (Email):</p>
            <input 
                value={userState.username}
                onChange={e => setUserState(prev => { return { ...prev, username: e.target.value } })}
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
            <button onClick={handleSignIn}>Sign in</button>
        </div>
    );
}

export default SignInView;