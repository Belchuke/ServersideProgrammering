import React, { useState } from 'react';
import { useMutation } from '@apollo/client';
import { CREATE_TO_DO} from '../api/ToDoEndpoints';

const CreateToDoView = () => {
    const [toDoState, setToDoState] = useState({
        name: '',
        description: '',
        toDoListId: 0
    });
    const [apiResponseState, setApiResponseState] = useState({
        message: '',
        isSuccessful: false
    });
    const [createToDoMutation] = useMutation(CREATE_TO_DO);

    let toDoListIds = [1, 2, 3]

    const handleCreate = () => {
        createToDoMutation({
            variables: {
                name: toDoState.name,
                description: toDoState.description,
                toDoListId: toDoState.toDoListId
            }
        })
        .then(res => {
            setApiResponseState({
                isSuccessful: res.data.createToDoList.isSuccessful,
                message:      res.data.createToDoList.message });
        })
        .catch(err => console.log(err));
    };

    if (localStorage.getItem('token') ==  null)
        return (<h2>Sign in to see create a to do</h2>)

    return (
        <div>
            <p>Name</p>
            <input 
                value={toDoState.name}
                onChange={e => setToDoState(prev => { return { ...prev, name: e.target.value } })}
                placeholder='To do name'
                type="text"
            />
            <p>Password:</p>
            <input
                value={toDoState.description}
                onChange={e => setToDoState(prev => { return { ...prev, description: e.target.value } })}
                placeholder='Description'
                type="text"
            /><br/>
            <select>
                <option value="">Choose list</option>
                { toDoListIds.map((value, index, array) => {
                    return <option value={value}>{value}</option>
                })}
            </select>
            <p>{apiResponseState.message}</p>
            <button onClick={handleCreate}>Create to do</button>
        </div>
    );
}
export default CreateToDoView;