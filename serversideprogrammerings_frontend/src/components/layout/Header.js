import React, { useCallback } from 'react';
import { Link, useNavigate } from 'react-router-dom';

const Header = ({ authState, setAuthState }) => {
	const signOut = () => {
		setAuthState(null)
	}

	return (
		<header style={{ display: 'flex', flexDirection: 'row', gap: '5px' }}>
			<Link to="/">My to do list</Link>
			<Link to="/create-todo-list">Create to do list</Link>
			{
				authState === null
					? <Link to="/sign-in">Sign in</Link>
					: <>{authState.username}<Link onClick={signOut}>Sign out</Link></>
			}
			<Link to="/register">Register</Link>
		</header>
	);
}

export default Header;
