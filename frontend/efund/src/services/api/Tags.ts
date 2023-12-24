import { Tag } from "../../models/Tag";
import API from "./repository/API";

const Tags = {
    async getTags(page: number = 1): Promise<Tag[]> {
        const response = await API.get(`/tags?page=${page}&pageSize=10`);
        return response.data as Tag[];
    },
};  

export default Tags;