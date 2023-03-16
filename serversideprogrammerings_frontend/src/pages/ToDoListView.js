import React, { useEffect, useState } from 'react';
import { useMutation, useQuery } from '@apollo/client';
import { CREATE_TO_DO, GET_USERS_TO_DO_LISTS } from '../api/ToDoEndpoints';

const ToDoListView = ({ authState }) => {
    const [toDoesState, setToDoesState] = useState([]);
    const [newToDoesState, setNewToDoesState] = useState({});
    const [createToDoMutation] = useMutation(CREATE_TO_DO);
    const { loading, error, data } = useQuery(GET_USERS_TO_DO_LISTS, {
        variables: {
            userId: Number(authState?.userId)
        }
    })

    useEffect(() => {
        let x = []
        // Dumb copy of the API data
        data.userToDoListsById.nodes.forEach(l => {
            l.listItems.nodes.forEach(i => {
                //setToDoesState(prev => { return { ...prev, [i.id]: { name: i.name, description: i.description } } })
                x[i.id] = { name: i.name, description: i.description }
            });
            //console.log(toDoesState)
        });
        setToDoesState(x)
    });

    const handleUpdate = () => {

    }

    const handleCreateToDo = (index, toDoListId) => {
        if (newToDoesState[toDoListId]?.name === undefined)
            newToDoesState[toDoListId].name = '';
        if (newToDoesState[toDoListId]?.description === undefined)
            newToDoesState[toDoListId].description = '';

        createToDoMutation({
            variables: {
                name: newToDoesState[toDoListId].name,
                description: newToDoesState[toDoListId].description,
                toDoListId: toDoListId
            }
        }).then(res => {
            if (res.data.createToDoListItem.isSuccessful)
                window.location.reload(); //TODO: figure out a way to update the component w/o a reload
            //data.userToDoListsById.nodes[index].listItems.nodes = [...data.userToDoListsById.nodes[index].listItems.nodes, {
            //    name: res.data.createToDoListItem.name,
            //    description: res.data.createToDoListItem.description
            //}];

            setNewToDoesState(prev => { return { ...prev, [toDoListId]: { ...newToDoesState[toDoListId], message: res.data.createToDoListItem.message } } });
        }).catch(err => console.log(err));
    }


    if (authState == null)
        return (<h2>Sign in to see your to do list</h2>);

    if (loading)
        return (<h2>Loading your to do list...</h2>);

    if (error)
        return (<h2>An error occurred while getting your to do list.</h2>);

    //let createNewToDoArr = Array.from({ length: data.userToDoListsById.nodes.length }, (l, i) => { return ({ name: '', description: '' }) });

    return (
        <>
            {data.userToDoListsById.nodes.map((toDoList, index, array) => {
                return (
                    <>
                        <h3>{toDoList.name}</h3>
                        <p>{toDoList.description}</p>
                        {toDoList.listItems.nodes.map((item) => {
                            <table>
                                <tbody>
                                    <tr>
                                        <th>Name</th>
                                        <th>Description</th>
                                        <th>Actions</th>
                                    </tr>
                                    <tr>
                                        <td>
                                            <input
                                                value={toDoesState[item.id].name}
                                                onChange={e => setToDoesState(prev => { return { ...prev, [item.id]: { ...toDoesState[item.id], name: e.target.value } } })}
                                            //onChange={e => x[item.id].name = e.target.value}
                                            />
                                        </td>
                                        <td><input value={item.description} /> </td>
                                        <td><button onClick={handleUpdate}>Update</button></td>
                                    </tr>
                                </tbody>
                            </table>
                        })}
                        <h5>Create a new to do</h5>
                        Name:
                        <input
                            value={newToDoesState[toDoList.id]?.name}
                            onChange={e => setNewToDoesState(prev => { return { ...prev, [toDoList.id]: { name: e.target.value, description: newToDoesState[toDoList.id]?.description } } })}
                            placeholder='To do name'
                            type="text"
                        />
                        Description:
                        <input
                            value={newToDoesState[toDoList.id]?.description}
                            onChange={e => setNewToDoesState(prev => { return { ...prev, [toDoList.id]: { name: newToDoesState[toDoList.id]?.name, description: e.target.value } } })}
                            placeholder='To do Description'
                            type="text"
                        />
                        <button onClick={() => handleCreateToDo(index, toDoList.id)}>Create to do</button>
                        <p>{newToDoesState[toDoList.id]?.message}</p>
                    </>
                )
            })}
        </>
    );
}

export default ToDoListView;