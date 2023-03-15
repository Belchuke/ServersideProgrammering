import { gql } from '@apollo/client';

export const CREATE_TO_DO_LIST = 
    gql`
        mutation CreateToDoList($name: String!, $description: String!, $items: [CreateToDoListItemInput!]) {
            createToDoList(input: {
                name: $name,
                description: $description,
                items: $items
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
                message
            }
        }
`;