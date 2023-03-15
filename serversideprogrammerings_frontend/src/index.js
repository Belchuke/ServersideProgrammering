import React from 'react';
import ReactDOM from 'react-dom/client';
import { ApolloClient, InMemoryCache, ApolloProvider } from '@apollo/client';
import App from "./App";
//import Routes from "./Routes";
import './index.css';

const apiUrl = "https://localhost:7228/graphql"

const client = new ApolloClient({
  uri: apiUrl,
  cache: new InMemoryCache(),
});

const root = ReactDOM.createRoot(document.getElementById('root'));

root.render(
    <ApolloProvider client={client} >
        <App />
    </ApolloProvider>
);
