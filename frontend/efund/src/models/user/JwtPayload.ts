interface JwtPayload {
    id: string;
    email: string;
    sub: string;
    jti: string;
    blocked: string;
    role: string;
    nbf: number;
    exp: number;
    iat: number;
    iss: string;
    aud: string;
}

export default JwtPayload;