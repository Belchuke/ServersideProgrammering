import React, { useState } from 'react';
import { useMutation } from '@apollo/client';
import { CREATE_TO_DO_LIST } from '../api/ToDoEndpoints';

const CreateToDoListView = () => {
    const [toDoListState, setToDoListState] = useState({
        name: '',
        description: ''
    });
    const [apiResponseState, setApiResponseState] = useState({
        message: '',
        isSuccessful: false
    });
    const [createToDoListMutation] = useMutation(CREATE_TO_DO_LIST);

    const handleCreate = () => {
        createToDoListMutation({
            variables: {
                name: toDoListState.name,
                description: toDoListState.description,
            }
        })
            .then(res => {
                setApiResponseState({
                    isSuccessful: res.data.createToDoList.isSuccessful,
                    message: res.data.createToDoList.message
                });
            })
            .catch(err => console.log(err));
    };

    if (localStorage.getItem('token') == null)
        return (<h2>Sign in to see create a to do</h2>)

    return (
        <div>
            <h2>Create to do list</h2>
            <p>Name</p>
            <input
                value={toDoListState.name}
                onChange={e => setToDoListState(prev => { return { ...prev, name: e.target.value } })}
                placeholder='Name'
                type="text"
            />
            <p>Description:</p>
            <input
                value={toDoListState.description}
                onChange={e => setToDoListState(prev => { return { ...prev, description: e.target.value } })}
                placeholder='Description'
                type="text"
            />
            <p>{apiResponseState.message}</p>
            <button onClick={handleCreate}>Create list</button>
        </div>
    );
}

export default CreateToDoListView;