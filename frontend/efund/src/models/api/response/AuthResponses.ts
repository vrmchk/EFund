import { APIResponseBase } from "../response/base/APIResponseBase";

export interface SignUpResponse extends APIResponseBase {
    userId: string;
}

export interface AuthSuccessResponse extends APIResponseBase {
    accessToken: string;
    refreshToken: string;
}