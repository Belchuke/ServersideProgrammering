import React, { useCallback, useEffect, useState } from 'react';
import { ApolloClient, InMemoryCache, ApolloProvider } from '@apollo/client';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Layout from './components/layout/Layout';
import ToDoListView from './pages/ToDoListView'
import CreateToDoListView from './pages/CreateToDoListView'
import SignInView from './pages/SignInView'
import RegisterView from './pages/RegisterView'
import RegisterTwoFactorAuthView from './pages/RegisterTwoFactorAuthView'
import SignInTwoFactorAuthView from './pages/SignInTwoFactorAuthView'
import NotFoundView from './pages/NotFoundView'

const apiUrl = "https://localhost:7228/graphql"

const App = () => {
	const [authState, setAuthState] = useState(JSON.parse(window.sessionStorage.getItem('authState')));
	useEffect(() => {
		window.sessionStorage.setItem('authState', JSON.stringify(authState));
	}, [authState]);

	const setAuthStateCallback = useCallback(e => {
		setAuthState(e);
	}, [authState]);

	const client = new ApolloClient({
		uri: apiUrl,
		headers: {
			Authorization: 'Bearer ' + authState?.token,
		},
		cache: new InMemoryCache(),
	});

	return (
		<ApolloProvider client={client} >
			<BrowserRouter>
				<Routes>
					<Route path='/' element={<Layout authState={authState} setAuthState={setAuthStateCallback} />}>
						<Route index element={<ToDoListView authState={authState} />} />
						<Route path="create-todo-list" element={<CreateToDoListView authState={authState} />} />
						<Route path="sign-in" element={<SignInView />} />
						<Route path="sign-in/two-factor-auth" element={<SignInTwoFactorAuthView setAuthState={setAuthStateCallback} />} />
						<Route path="register" element={<RegisterView />} />
						<Route path="register/two-factor-auth" element={<RegisterTwoFactorAuthView />} />
						<Route path="*" element={<NotFoundView />} />
					</Route>
				</Routes>
			</BrowserRouter>
		</ApolloProvider>
	);
}

export default App;
