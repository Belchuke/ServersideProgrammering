import { gql } from '@apollo/client';

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
export const REGISTER_TWO_FACTOR_AUTH =
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
export const SIGN_IN =
    gql`
        mutation SignIn($username: String!, $password: String!) {
            signIn(input: {
                username: $username,
                password: $password
            }) {
                isSuccessful,
                message
            }
        }
`;
export const SIGN_IN_TWO_FACTOR_AUTH =
    gql`
        mutation TwoFactorSignIn($username: String!, $code: String!) {
            toFactorSignIn(input: {
                username: $username,
                code: $code
            }) {
                isSuccessful,
                message,
                userId,
                user {
                    username
                }
                token,
                expires,
                refreshToken
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
