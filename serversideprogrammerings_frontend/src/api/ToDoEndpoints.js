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