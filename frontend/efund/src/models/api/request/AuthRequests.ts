import { APIRequestBase } from "./base/APIRequestBase";

export interface SignUpRequest extends APIRequestBase {
    name: string;
    email: string;
    password: string;
    adminToken?: string;
}

export interface ConfirmEmailRequest extends APIRequestBase { 
    userId: string;
    code: number;
}


export { }