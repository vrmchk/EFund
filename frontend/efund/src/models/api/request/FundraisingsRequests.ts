import { PagedRequest } from "./base/PagedRequest";

export interface SearchFundraisingsRequest extends PagedRequest {
    title?: string;
    tags?: string[];
    includeClosed?: boolean;
}