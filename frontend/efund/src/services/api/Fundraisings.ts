import Fundraising from "../../models/Fundraising";
import { SearchFundraisingsRequest } from "../../models/api/request/FundraisingsRequests";
import Paged from "../../models/api/response/base/Paged";
import API from "./repository/API";

const Fundraisings = {
    async getFundraisings(request: SearchFundraisingsRequest): Promise<Paged<Fundraising> | undefined> {
        const response = await API.postPaged<SearchFundraisingsRequest, Paged<Fundraising>>('/fundraisings/search', request);

        return response.data;
    },
};

export default Fundraisings;