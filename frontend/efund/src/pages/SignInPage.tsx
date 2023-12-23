import { Paper, Typography, Box } from "@mui/material";
import useNotification from "../hooks/useNotification";
import '../styles/sign-in.css';
import SignInForm from "../components/auth/sign-in/SignInForm";
import { SignInFormFields } from "../models/form/auth/AuthFormFields";
import Auth from "../services/api/auth/Auth";
import { useNavigate } from "react-router-dom";
import useUser from "../hooks/useUser";

const SignInPage = () => {
    const { updateUser } = useUser();
    const { notifyError, Notification } = useNotification();
    const navigate = useNavigate();

    const onSubmit = async (fields: SignInFormFields) => {
        const result = await Auth.signIn({ ...fields });
        if (!result) {
            const user = await Auth.me();
            if (user)
                updateUser(user);
            else
                notifyError('Error during signing in');
            navigate('/');
        }

        notifyError(result?.message ?? 'Error during signing in');
    };
    return (
        <>
            <Paper
                elevation={3}
                className="sign-in-paper">
                <Box>
                    <Box className="sign-in-header">
                        <Typography
                            className="sign-in-title"
                            variant='h4'>
                            Sign In
                        </Typography>
                    </Box>
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            alignItems: 'center',
                            textAlign: 'center'
                        }}>
                        <SignInForm onSubmit={onSubmit} />
                    </Box>
                </Box>
                <Box sx={{
                    width: '100%',
                    position: 'absolute',
                    bottom: '15px',
                }}>
                </Box>
            </Paper >
            <Notification />
        </>
    )
};

export default SignInPage;