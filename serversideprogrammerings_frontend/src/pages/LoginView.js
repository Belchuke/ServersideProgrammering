import React from 'react';
import { useQuery, useMutation } from '@apollo/client';

const handleLogin = async e => {
//    e.preventDefault();
//    const token = await loginUser({
//      username,
//      password
//    });
    //setToken(token);
  }

const LoginView = () => {
    return (

        <form onSubmit={handleLogin}>
            <label>
                Username (Email): <input name='username' type="text" />
            </label>
            <label>
                Password: <input name='password' type="text" />
            </label>
            <button type='submit'>Login</button>
        </form>
    );
}

export default LoginView;