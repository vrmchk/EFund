import { Box, Pagination, Skeleton } from "@mui/material";
import PageWrapper from "../components/common/PageWrapper";
import Search from "../components/common/Search";
import '../styles/home-page.css';
import { useEffect, useState } from "react";
import useUser from "../hooks/useUser";
import Tags from '../services/api/Tags';
import { Tag } from "../models/Tag";
import MultiSelect from "../components/common/MultiSelectWithChips";
import Fundraisings from "../services/api/Fundraisings";
import Fundraising from "../models/Fundraising";
import FundraisingCard from "../components/common/FundraisingCard";

const HomePage = () => {
    const [tags, setTags] = useState<Tag[]>([]);
    const [loading, setLoading] = useState<boolean>(false);

    const [fundraisings, setFundraisings] = useState<Fundraising[]>([]);
    const [selectedFundraising, setSelectedFundraising] = useState<Fundraising>();

    const [selectedTags, setSelectedTags] = useState<string[]>([]);
    const [searchQuery, setSearchQuery] = useState<string>('');

    const [page, setPage] = useState<number>(1);
    const [totalPages, setTotalPages] = useState<number>(1);

    const { user } = useUser();

    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            const fundraisings = await Fundraisings.getFundraisings({ page: page, pageSize: 2, tags: selectedTags, title: searchQuery });
            console.log(fundraisings?.items);
            if (fundraisings && fundraisings?.items) {
                setFundraisings(fundraisings!.items);
                setTotalPages(fundraisings!.totalPages);
            }
            else {
                setFundraisings([]);
                setTotalPages(1);
            }
            setLoading(false);
        }

        fetchData();
    }, [selectedTags, searchQuery, page]);

    useEffect(() => {
        const fetchData = async () => {
            const tags = await Tags.getTags();
            if (tags) {
                setTags(tags);
            }
        }

        fetchData();
    }, [user]);

    return (
        <PageWrapper>
            <Box className='home-page-content'>
                <Box sx={
                    {
                        display: 'flex',
                        alignItems: 'center',
                        flexDirection: 'row',
                        gap: '25px',
                    }
                }>
                    <Search onSearch={(query) => setSearchQuery(query)} />
                    <MultiSelect names={tags.map((tag) => tag.name)} onChange={setSelectedTags} />
                </Box >
                <Box className='search-result-container'>
                    <div style={{
                        display: 'flex',
                        flexDirection: 'column',
                        gap: '20px',
                    }}>
                        {
                            fundraisings.length === 0 && !loading
                                ? <h3>No fundraisings found</h3>
                                : fundraisings.map((fundraising, index) => (
                                    <>
                                        <Box height={'140px'} key={index}>
                                            {loading
                                                ? <Skeleton sx={{ transform: 'scale(1, 0.90)', height: '100%' }} />
                                                : <FundraisingCard fundraising={fundraising} />}
                                        </Box>
                                    </>
                                ))
                        }
                    </div>
                    <Pagination sx={{
                        display: totalPages > 1 ? 'flex' : 'none',
                    }} count={totalPages} page={page} onChange={(_, value) => setPage(value)} />
                </Box>
            </Box>
        </PageWrapper>
    );
};

export default HomePage;