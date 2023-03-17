import React, { useEffect, useState } from 'react';
import { useMutation, useQuery } from '@apollo/client';
import { CREATE_TO_DO, DELETE_TO_DO_ITEM, GET_USERS_TO_DO_LISTS, UPDATE_TO_DO_ITEM, DELETE_TO_DO_LIST } from '../api/ToDoEndpoints';

const ToDoListView = ({ authState }) => {
    const [toDoesState, setToDoesState] = useState([]);
    const [newToDoesState, setNewToDoesState] = useState({});
    const [createToDoMutation] = useMutation(CREATE_TO_DO);
    const [updateToDoMutation] = useMutation(UPDATE_TO_DO_ITEM);
    const [deleteToDoMutation] = useMutation(DELETE_TO_DO_ITEM);
    const [deleteToDoListMutation] = useMutation(DELETE_TO_DO_LIST);
    const { loading, error, data } = useQuery(GET_USERS_TO_DO_LISTS, {
        variables: {
            userId: Number(authState?.userId)
        },
        async onCompleted(data) {
            let items = []
            data.userToDoListsById.nodes.forEach(l => {
                l.listItems.nodes.forEach(i => {
                    items[i.id] = { id: i.id, name: i.name, description: i.description }
                });
            })
            setToDoesState(items)
        }
    })

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
                window.location.reload();

            setNewToDoesState(prev => { return { ...prev, [toDoListId]: { ...newToDoesState[toDoListId], message: res.data.createToDoListItem.message } } });
        }).catch(err => console.log(err));
    }


    const handleDeleteList = (toDoListId) => {
        deleteToDoListMutation({
            variables: {
                id: toDoListId
            }
        }).then(res => {
            if (res.data.updateToDoList.isSuccessful)
                window.location.reload();

            setNewToDoesState(prev => { return { ...prev, [toDoListId]: { ...newToDoesState[toDoListId], message: res.data.updateToDoList.message } } });
        }).catch(err => console.log(err));
    }

    if (authState == null)
        return (<h2>Sign in to see your to do list</h2>);

    if (loading)
        return (<h2>Loading your to do list...</h2>);

    if (error)
        return (<h2>An error occurred while getting your to do list.</h2>);

    const RenderToDoItem = ({ item, toDoListId }) => {
        if (item === undefined)
            return;

        const handleUpdate = () => {
            updateToDoMutation({
                variables: {
                    id: item.id,
                    name: item.name,
                    description: item.description,
                }
            }).then(res => {
                if (res.data.updateToDoListItem.isSuccessful)
                    window.location.reload();

                setNewToDoesState(prev => { return { ...prev, [toDoListId]: { ...newToDoesState[toDoListId], message: res.data.updateToDoListItem.message } } });
            }).catch(err => console.log(err));
        }
        const handleDelete = () => {
            deleteToDoMutation({
                variables: {
                    id: item.id
                }
            }).then(res => {
                if (res.data.updateToDoListItem.isSuccessful)
                    window.location.reload();

                setNewToDoesState(prev => { return { ...prev, [toDoListId]: { ...newToDoesState[toDoListId], message: res.data.updateToDoListItem.message } } });
            }).catch(err => console.log(err));
        }

        return (
            <tr>
                <td>
                    <input
                        value={item.name}
                        onChange={e => setToDoesState(prev => { return { ...prev, [item.id]: { ...toDoesState[item.id], name: e.target.value } } })}
                        type="text"
                    />
                </td>
                <td>
                    <input
                        value={item.description}
                        onChange={e => setToDoesState(prev => { return { ...prev, [item.id]: { ...toDoesState[item.id], description: e.target.value } } })}
                        type="text"
                    />
                </td>
                <td>
                    <button onClick={handleUpdate}>Update</button>
                    <button onClick={handleDelete}>Delete</button>
                </td>
            </tr>
        );
    }

    return (
        <>
            {data.userToDoListsById.nodes.map((toDoList, index, array) => {
                return (
                    <>
                        <h3>{toDoList.name}</h3>
                        <p>{toDoList.description}</p>
                        <button onClick={() => handleDeleteList(toDoList.id)}>Delete list</button>
                        <table>
                            <tbody>
                                <tr>
                                    <th>Name</th>
                                    <th>Description</th>
                                    <th>Actions</th>
                                </tr>
                                {toDoList.listItems.nodes.map((item) => RenderToDoItem({ item: toDoesState[item.id], toDoListId: toDoList.id }))}
                            </tbody>
                        </table>
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