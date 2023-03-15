import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Layout from './components/layout/Layout';
import ToDoListView from './pages/ToDoListView'
import CreateTodoView from './pages/CreateTodoView'
import LoginView from './pages/LoginView'
import RegisterView from './pages/RegisterView'
import TwoFactorAuthView from './pages/TwoFactorAuthView'
import NotFoundView from './pages/NotFoundView'

//import { GET_USERS, REGISTER_USER } from './api/UserQueries';
//import { useQuery, gql } from '@apollo/client';

const App = () => {
	//const {loading, er, data} = useQuery(GET_USERS());

	//if (!loading)
	//	console.log(data.requiredQuery.name);
	return (
		<BrowserRouter>
        	<Routes>
				<Route path='/' element={<Layout />}>
					<Route index element={<ToDoListView />} />
        	    	<Route path="create-todo" element={<CreateTodoView />} />
        	    	<Route path="login" element={<LoginView />} />
        	    	<Route path="register" element={<RegisterView />} />
        	    	<Route path="two-factor-auth" element={<TwoFactorAuthView />} />
					<Route path="*" element={<NotFoundView />} />
				</Route>
        	</Routes>
		</BrowserRouter>
	);
}

export default App;
