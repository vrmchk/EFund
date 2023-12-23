import { Box, Card, Link } from "@mui/material";
import { useNavigate } from "react-router-dom";
import useUser from "../../hooks/useUser";
import MenuAvatar from "../home/MenuAvatar";

const Header = () => {

    const { user, logout, loading } = useUser();
    const navigate = useNavigate();

    return (
        <Card className='header'
            sx={{
                position: 'relative',
                display: 'flex',
                flexDirection: 'row',
                justifyContent: 'left',
                alignItems: 'center',
                padding: '0 50px',
                width: '100%',
                height: '70px',
            }}>
            <Link
                href='/'
                variant="h6"
                color="inherit"
                underline="none">EFund</Link>
            <Box
                sx={{
                    position: 'absolute',
                    right: '50px',
                    display: 'flex',
                    flexDirection: 'row',
                    justifyContent: 'space-between',
                    alignItems: 'center',
                    width: 'max-content',
                    gap: '25px'
                }}>
                {loading
                    ? <></>
                    : (user ?
                        <MenuAvatar
                            onSignOut={logout}
                            onSettings={() => navigate('/settings')} />
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
    );
};

export default Header;