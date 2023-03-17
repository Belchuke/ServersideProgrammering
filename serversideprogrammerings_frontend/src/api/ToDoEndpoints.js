import { gql } from '@apollo/client';

export const CREATE_TO_DO_LIST =
    gql`
        mutation CreateToDoList($name: String!, $description: String!) {
            createToDoList(input: {
                name: $name,
                description: $description
            }) {
                isSuccessful,
                message
            }
        }
`;

export const CREATE_TO_DO =
    gql`
        mutation CreateToDoListItem($name: String!, $description: String!, $toDoListId: Long) {
            createToDoListItem(input: {
                name: $name,
                description: $description,
                toDoListId: $toDoListId
            }) {
                id,
                isSuccessful,
                message,
                name,
                description
            }
        }
`;

export const GET_USERS_TO_DO_LIST_ID_NAMES =
    gql`
        query GetUsersToDoListNames($userId: Long!) {
            userToDoListsById(userId: $userId) {
                nodes {
                    id,
                    name
                }
            }
        }
`;

export const GET_USERS_TO_DO_LISTS =
    gql`
        query GetUsersToDoListNames($userId: Long!) {
            userToDoListsById(userId: $userId) {
                nodes {
                    id
                    name
                    description
                    listItems {
                        nodes {
                            id,
                            name,
                            description
                        }
                    }
                }
            }
        }
`;

export const UPDATE_TO_DO_ITEM =
    gql`
        mutation UpdateToDoListItem($id: Long, $name: String, $description: String) {
            updateToDoListItem(input: {
                id: $id,
                name: $name,
                description: $description,
                addNewItem: false
            }) {
                isSuccessful,
                message,
                id,
                name,
                description
            }
        }
`;

export const DELETE_TO_DO_ITEM =
    gql`
        mutation DeleteToDoListItem($id: Long) {
            updateToDoListItem(input: {
                id: $id,
                delete: true,
                addNewItem: false
            }) {
                isSuccessful,
                message
            }
        }
`;

export const DELETE_TO_DO_LIST =
    gql`
        mutation DeleteToDoList($id: Long!) {
            updateToDoList(input: {
                id: $id,
                delete: true
            }) {
                isSuccessful,
                message
            }
        }
`;