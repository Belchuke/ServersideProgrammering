import React from 'react';
import { useQuery } from '@apollo/client';
import { GET_USERS_TO_DO_LISTS} from '../api/ToDoEndpoints';

const ToDoListView = () => {
    const { loading, error, data } = useQuery(GET_USERS_TO_DO_LISTS)


    
    if (localStorage.getItem('token') ==  null)
        return (<h2>Sign in to see your to do list</h2>);

    if (loading)
        return (<h2>Loading your to do list...</h2>);

    if (error)
        return (<h2>An error occurred while getting your to do list.</h2>);
    
    console.log(data)

    return (
        <>
            <table>
                <tbody>
                    <tr>
                        <th>Name</th>
                        <th>Description</th>
                    </tr>
                    <tr>
                        <td> </td>
                        <td> </td>
                    </tr>
                </tbody>
            </table>
        </>
    );
}

export default ToDoListView;