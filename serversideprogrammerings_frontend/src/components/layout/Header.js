import React from 'react';
import { Link } from 'react-router-dom';

const Header = () => {
	return (
		<header>
			<Link to="/">To do list</Link>
			<Link to="/create-todo">Create to do</Link>
			<Link to="/login">Login</Link>
			<Link to="/register">Register</Link>
		</header>
	);
}

export default Header;
