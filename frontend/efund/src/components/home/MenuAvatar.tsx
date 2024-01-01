import Logout from "@mui/icons-material/Logout";
import Settings from "@mui/icons-material/Settings";
import { Avatar, ListItemIcon, Divider, Menu, MenuItem, Typography } from '@mui/material';
import { useState } from "react";
import Person2Icon from '@mui/icons-material/Person2';
import useUser from "../../hooks/useUser";
import { stringAvatar } from "../../services/utils/convert";

interface MenuAvatarProps {
    onSignOut: () => void;
    onSettings: () => void;
    onProfile: () => void;
}

const MenuAvatar = (props: MenuAvatarProps) => {
    const { user } = useUser();

    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
    const open = Boolean(anchorEl);

    const handleClick = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };
    const handleClose = () => {
        setAnchorEl(null);
    };

    return (
        <>
            <Typography mr={-1} variant="h6">{user?.name}</Typography>
            <Avatar
                onClick={handleClick}
                {...stringAvatar(user?.name)}
                src={user?.avatarUrl}
            />
            <Menu
                anchorEl={anchorEl}
                id="account-menu"
                open={open}
                onClose={handleClose}
                sx={{ width: '220px !important' }}
                PaperProps={{
                    elevation: 0,
                    sx: {
                        overflow: 'visible',
                        filter: 'drop-shadow(0px 2px 8px rgba(0,0,0,0.32))',
                        mt: 1.5,
                        '& .MuiAvatar-root': {
                            width: 32,
                            height: 32,
                            ml: -0.5,
                            mr: 1,
                        },
                        '&:before': {
                            content: '""',
                            display: 'block',
                            position: 'absolute',
                            top: 0,
                            right: 14,
                            width: 10,
                            height: 10,
                            bgcolor: 'background.paper',
                            transform: 'translateY(-50%) rotate(45deg)',
                            zIndex: 0,
                        },
                    },
                }}
                transformOrigin={{ horizontal: 'right', vertical: 'top' }}
                anchorOrigin={{ horizontal: 'right', vertical: 'bottom' }}
            >
                <MenuItem onClick={props.onProfile}>
                    <ListItemIcon>
                        <Person2Icon fontSize="small" />
                    </ListItemIcon>
                    Profile
                </MenuItem>
                <Divider />
                <MenuItem onClick={props.onSettings}>
                    <ListItemIcon>
                        <Settings fontSize="small" />
                    </ListItemIcon>
                    Settings
                </MenuItem>
                <MenuItem onClick={props.onSignOut}>
                    <ListItemIcon>
                        <Logout fontSize="small" />
                    </ListItemIcon>
                    Logout
                </MenuItem>
            </Menu >
        </>
    );
};

export default MenuAvatar;