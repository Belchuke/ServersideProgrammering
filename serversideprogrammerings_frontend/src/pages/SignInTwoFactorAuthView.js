import React, { useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom'
import { useMutation } from '@apollo/client';
import { SIGN_IN_TWO_FACTOR_AUTH } from '../api/UserEndpoints';

const SignInTwoFactorAuthView = ({ setAuthState }) => {
    const location = useLocation();
    const navigate = useNavigate();
    const [userAuthState, setUserAuthState] = useState({
        username: location.state?.username !== undefined ? location.state.username : '',
        twoFactorCode: ''
    });
    const [apiResponseState, setApiResponseState] = useState({
        message: '',
        isSuccessful: false
    });
    const [twoFactorAuthMutation] = useMutation(SIGN_IN_TWO_FACTOR_AUTH);

    const handleSubmit = () => {
        twoFactorAuthMutation({
            variables: {
                username: userAuthState.username,
                code: userAuthState.twoFactorCode
            }
        })
            .then(res => {
                if (res.data.toFactorSignIn.isSuccessful) {
                    //localStorage.setItem('token', res.data.toFactorSignIn.token);
                    //localStorage.setItem('userId', res.data.toFactorSignIn.userId);
                    setAuthState({
                        username: res.data.toFactorSignIn.user.username,
                        userId: res.data.toFactorSignIn.userId,
                        token: res.data.toFactorSignIn.token
                    });
                    navigate('/');
                }
                else
                    setApiResponseState({
                        isSuccessful: res.data.toFactorSignIn.isSuccessful,
                        message: res.data.toFactorSignIn.message
                    });
            })
            .catch(err => console.log(err));
    };

    return (
        <div>
            <h3>A two factor authorization code has been sent to your email</h3>
            {location.state?.username !== undefined
                ? <h4>{location.state.username}</h4>
                :
                <>
                    <p>Username (Email):</p>
                    <input
                        value={userAuthState.username}
                        onChange={e => setUserAuthState(prev => { return { ...prev, username: e.target.value } })}
                        placeholder='Username'
                        type="text"
                    />
                </>
            }

            <p>Enter the code here:</p>
            <input
                value={userAuthState.twoFactorCode}
                onChange={e => setUserAuthState(prev => { return { ...prev, twoFactorCode: e.target.value } })}
                placeholder='Two factor code'
                type="text"
            />
            {!apiResponseState.isSuccessful ? <p>{apiResponseState.message}</p> : false}
            <button onClick={handleSubmit}>Submit</button>
        </div>
    );
}

export default SignInTwoFactorAuthView;