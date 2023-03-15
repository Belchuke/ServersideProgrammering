import React from 'react';

const ToDoListView = () => {
    if (localStorage.getItem('token') ==  null)
        return (<h2>Sign in to see your to do list</h2>)

    

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