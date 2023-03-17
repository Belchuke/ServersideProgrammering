import React, { useState } from 'react';
import { useMutation } from '@apollo/client';
import { CREATE_TO_DO_LIST } from '../api/ToDoEndpoints';
import { useNavigate } from 'react-router-dom';

const CreateToDoListView = ({ authState }) => {
    const navigate = useNavigate();
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
                if (res.data.createToDoList.isSuccessful)
                    window.location.reload();

                setApiResponseState({
                    isSuccessful: res.data.createToDoList.isSuccessful,
                    message: res.data.createToDoList.message
                });
            })
            .catch(err => console.log(err));
    };

    if (authState == null)
        return (<h2>Sign in to create a to do list</h2>)

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