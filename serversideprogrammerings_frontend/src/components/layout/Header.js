import React from 'react';
import { Link, useNavigate } from 'react-router-dom';

const Header = () => {
	const navigate = useNavigate();
	const signOut = () => {
		localStorage.removeItem('token');
		navigate('/');
	}

	return (
		<header>
			<Link to="/">To do list</Link>
			<Link to="/create-todo">Create to do</Link>
			{
				localStorage.getItem('token') == null
				? <Link to="/sign-in">Sign in</Link>
				: <Link onClick={signOut}>Sign out</Link>
			}
			<Link to="/register">Register</Link>
		</header>
	);
}

export default Header;
