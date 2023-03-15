import { gql } from '@apollo/client';
//
////const client = new ApolloClient({
////  uri: apiUrl,
////  cache: new InMemoryCache()
////});
//
//export const registerUser = (userName, password) => {
//    return gql`
//        mutation  {
//            registerUser(input: {
//                "${userName}",
//                "${password}"
//            }) {
//                isSuccessful
//            }
//        }
//`;
//}

//TODO: there is no type checking on the parameters. Figure out a neat way to do it.

export const REGISTER_USER = 
    gql`
        mutation RegisterUser($userName: String!, $password: String!) {
            registerUser(input: {
                userName: $userName,
                password: $password
            }) {
                isSuccessful,
                message
            }
        }
`;
export const TWO_FACTOR_AUTH = 
    gql`
        mutation ValidateOTP($username: String!, $code: String!) {
            validateOTP(input: {
                username: $username,
                code: $code
            }) {
                isSuccessful,
                message
            }
        }
`;


export const GET_USERS = () => {
return gql`
query r {
    requiredQuery{
        name
    }
}
`;
}


//const GET_PROPERTY = gql
//  query GetProperty {
//    property {
//      id
//      name
//      address
//      city
//      state
//      zip
//    }
//  };

//export const [registerUser, { data, loading, error }] = useMutation(REGISTER_USER);

//client.query({
//  query: GET_PROPERTY
//}).then(result => console.log(result.data.property));
