import API from '../repository/API';
import { ConfirmEmailRequest, SignUpRequest } from '../../../models/api/request/AuthRequests';
import { AuthSuccessResponse, SignUpResponse } from '../../../models/api/response/AuthResponses';

const Auth = {
    signUp: async (request: SignUpRequest) : Promise<SignUpResponse> => {
        const response = await API.post<SignUpRequest, SignUpResponse>('/auth/sign-up', request);
        
        if (response.success) {
            return response.data as SignUpResponse;
        }

        throw new Error(response.error?.message ?? 'Unknown error');
    },

    confirmEmail: async (request: ConfirmEmailRequest): Promise<AuthSuccessResponse> => {
        const response = await API.post<ConfirmEmailRequest, AuthSuccessResponse>('/auth/confirm-email', request);

        if (response.success) {
            return response.data as AuthSuccessResponse;
        }

        throw new Error(response.error?.message ?? 'Unknown error');
    }
};

export default Auth;