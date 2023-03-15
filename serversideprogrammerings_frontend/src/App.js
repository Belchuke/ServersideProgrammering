import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Layout from './components/layout/Layout';
import ToDoListView from './pages/ToDoListView'
import CreateToDoView from './pages/CreateToDoView'
import CreateToDoListView from './pages/CreateToDoListView'
import SignInView from './pages/SignInView'
import RegisterView from './pages/RegisterView'
import RegisterTwoFactorAuthView from './pages/RegisterTwoFactorAuthView'
import SignInTwoFactorAuthView from './pages/SignInTwoFactorAuthView'
import NotFoundView from './pages/NotFoundView'

const App = () => {
	//const {loading, er, data} = useQuery(GET_USERS());

	//if (!loading)
	//	console.log(data.requiredQuery.name);
	return (
		<BrowserRouter>
        	<Routes>
				<Route path='/' element={<Layout />}>
					<Route index element={<ToDoListView />} />
        	    	<Route path="create-todo" element={<CreateToDoView />} />
        	    	<Route path="create-todo-list" element={<CreateToDoListView />} />
        	    	<Route path="sign-in" element={<SignInView />} />
        	    	<Route path="sign-in/two-factor-auth" element={<SignInTwoFactorAuthView />} />
        	    	<Route path="register" element={<RegisterView />} />
        	    	<Route path="register/two-factor-auth" element={<RegisterTwoFactorAuthView />} />
					<Route path="*" element={<NotFoundView />} />
				</Route>
        	</Routes>
		</BrowserRouter>
	);
}

export default App;
