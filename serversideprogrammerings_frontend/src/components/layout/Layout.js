import { useCallback } from 'react';
import { Outlet } from "react-router-dom";
import Header from './Header';
import Footer from './Footer';

const Layout = ({ authState, setAuthState }) => {
  return (
    <>
      <Header authState={authState} setAuthState={setAuthState} />
      <Outlet />
      <Footer />
    </>
  );
};

export default Layout;
