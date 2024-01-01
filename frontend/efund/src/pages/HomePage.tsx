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
    const [selectedLoading, setSelectedLoading] = useState<boolean>(false);

    const [fundraisings, setFundraisings] = useState<Fundraising[]>([]);
    const [selectedFundraisingId, setSelectedFundraisingId] = useState<string | undefined>();
    const [selectedFundraising, setSelectedFundraising] = useState<Fundraising | undefined>();

    const [selectedTags, setSelectedTags] = useState<string[]>([]);
    const [searchQuery, setSearchQuery] = useState<string>('');

    const [page, setPage] = useState<number>(1);
    const [totalPages, setTotalPages] = useState<number>(1);

    const { user } = useUser();

    const pageSize = 3;

    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            const fundraisings = await Fundraisings.getFundraisings({ page: page, pageSize: pageSize, tags: selectedTags, title: searchQuery });
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
        setPage(1);
    }, [selectedTags, searchQuery]);

    useEffect(() => {
        const fetchData = async () => {
            const tags = await Tags.getTags();
            if (tags) {
                setTags(tags);
            }
        }

        fetchData();
    }, [user]);

    useEffect(() => {
        const fetchData = async () => {
            setSelectedLoading(true);
            if (selectedFundraisingId) {
                const fundraising = await Fundraisings.getFundraising(selectedFundraisingId);
                if (fundraising) {
                    setSelectedFundraising(fundraising);
                }
            }
            setSelectedLoading(false);
        }

        fetchData();
    }, [selectedFundraisingId]);

    return (
        <PageWrapper>
            <Box className='home-page-content'>
                <Box sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    gap: '20px'
                }}>
                    <Box sx={
                        {
                            display: 'flex',
                            alignItems: 'center',
                            flexDirection: 'row',
                            gap: '25px',
                            justifyContent: 'space-evenly',
                        }
                    }>
                        <Search onSearch={(query) => setSearchQuery(query)} />
                        <MultiSelect names={tags.map((tag) => tag.name)} onChange={setSelectedTags} />
                    </Box >
                    <Box className='search-result-container'>
                        <div style={{
                            display: 'flex',
                            flexDirection: 'column',
                            alignItems: 'center',
                            minWidth: 600,
                            gap: '20px',
                        }}>
                            {
                                loading
                                    ? (Array(pageSize).fill(0).map((_, index) => (
                                        <Skeleton key={index} sx={{ transform: 'scale(1, 0.90)', height: '130px', width: 600 }} />
                                    )))
                                    : (
                                        !loading && fundraisings.length === 0
                                            ? <h3>No fundraisings found</h3>
                                            : (fundraisings.map((fundraising, index) => (
                                                <Box height={'130px'} key={index}>
                                                    <FundraisingCard
                                                        selected={fundraising.id === selectedFundraisingId}
                                                        key={index}
                                                        onClick={(setSelectedFundraisingId)}
                                                        fundraising={fundraising}
                                                        size="small" />
                                                </Box>
                                            ))))
                            }
                        </div>
                        <Pagination sx={{
                            display: totalPages > 1 ? 'flex' : 'none',
                            justifyContent: 'center',
                        }} count={totalPages} page={page} onChange={(_, value) => setPage(value)} />
                    </Box>
                </Box>
                <Box className='selected-fundraising-container'>
                    {
                        selectedLoading
                            ? <Skeleton sx={{ height: '100%', width: '100%', transform: 'scale(1, 0.95)' }} />
                            : (
                                selectedFundraising &&
                                <FundraisingCard
                                    fundraising={selectedFundraising}
                                    size="large" />
                            )
                    }
                </Box>
            </Box>
        </PageWrapper>
    );
};

export default HomePage;