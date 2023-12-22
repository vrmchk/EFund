import axios from 'axios';
import { APIRequestBase } from '../../../models/api/request/base/APIRequestBase';
import { ErrorModel } from '../../../models/api/response/base/ErrorModel';

const API_URL = 'http://localhost:5282/api';

interface APIResponse<T> {
    success: boolean;
    data?: T;
    error?: ErrorModel;
}

export const API = {
    get: async <TResponse>(url: string): Promise<APIResponse<TResponse>> => {
        try {
            const response = await axios.get<TResponse>(API_URL + url, {
                headers: {
                    'Authorization': 'Bearer ' + localStorage.getItem('accessToken')
                }
            });
            return { success: true, data: response.data };
        } catch (error: any) {
            return { success: false, error: error.response?.data };
        }
    },

    post: async <TRequest extends APIRequestBase, TResponse>(
        url: string,
        data: TRequest
    ): Promise<APIResponse<TResponse>> => {
        try {
            const response = await axios.post<TResponse>(API_URL + url, data, {
                headers: {
                    'Authorization': 'Bearer ' + localStorage.getItem('accessToken')
                }
            });
            return { success: true, data: response.data };
        } catch (error: any) {
            return { success: false, error: error.response?.data };
        }
    },

    put: async <TRequest extends APIRequestBase, TResponse>(
        url: string,
        data: TRequest
    ): Promise<APIResponse<TResponse>> => {
        try {
            const response = await axios.put<TResponse>(API_URL + url, data, {
                headers: {
                    'Authorization': 'Bearer ' + localStorage.getItem('accessToken')
                }
            });
            return { success: true, data: response.data };
        } catch (error: any) {
            return { success: false, error: error.response?.data };
        }
    },

    delete: async <TResponse>(url: string): Promise<APIResponse<TResponse>> => {
        try {
            const response = await axios.delete<TResponse>(API_URL + url, {
                headers: {
                    'Authorization': 'Bearer ' + localStorage.getItem('accessToken')
                }
            });
            return { success: true, data: response.data };
        } catch (error: any) {
            return { success: false, error: error.response?.data };
        }
    },
};

export default API;