import { Box, Card, Link } from "@mui/material";
import { useNavigate } from "react-router-dom";
import useUser from "../../hooks/useUser";
import MenuAvatar from "../home/MenuAvatar";
import { ReactNode } from "react";
import '../../styles/page-wrapper.css';

interface PageWrapperProps {
    children: ReactNode;
}

const PageWrapper = ({ children }: PageWrapperProps) => {
    const { user, logout, loading } = useUser();
    const navigate = useNavigate();

    return (
        <Box
            className='page-wrapper'>
            <Card className='header'>
                <Link
                    href='/'
                    variant="h6"
                    color="inherit"
                    underline="none">EFund</Link>
                <Box
                    className='header-actions'>
                    {loading
                        ? <></>
                        : (user ?
                            <MenuAvatar
                                onSignOut={logout}
                                onSettings={() => navigate('/settings')}
                                onProfile={() => navigate('/profile')}
                            />
                            :
                            <>
                                <Link
                                    href='/sign-in'
                                    variant="h6"
                                    color="inherit"
                                    underline="none">Sign In</Link>
                                <Link
                                    href='/sign-up'
                                    variant="h6"
                                    color="inherit"
                                    underline="none">Sign Up</Link>
                            </>
                        )}
                </Box>
            </Card>
            <Box
                sx={{ flexGrow: 1 }}>
                {children}
            </Box>
            <Card
                className='footer'>
                Footer
            </Card>
        </Box>
    );
};

export default PageWrapper;