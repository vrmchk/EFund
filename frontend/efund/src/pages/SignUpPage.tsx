import React, { useEffect } from "react";
import Auth from "../services/api/auth/Auth";
import SignUpForm from "../components/auth/sign-up/SignUpForm";
import { Box, Paper, Step, StepLabel, Typography } from "@mui/material";
import { ConfirmEmailRequest, SignUpRequest } from '../models/api/request/AuthRequests';
import { useNavigate } from 'react-router-dom';
import Stepper from '@mui/material/Stepper';
import EmailConfirmForm from "../components/auth/sign-up/EmailConfirmForm";
import useNotification from "../hooks/useNotification";
import '../styles/sign-up.css';
import useUser from "../hooks/useUser";

const SignUpPage = () => {
    const { updateUser } = useUser();
    const navigate = useNavigate();

    const {
        notifySuccess,
        notifyError,
        Notification } = useNotification();

    const signUp = async (request: SignUpRequest) => {
        Auth.signUp(request)
            .then((response) => {
                setUserId(response.userId);
            })
            .catch(() => {
                notifyError("Error during signing up");
            });
    }

    const confirmEmail = async (request: ConfirmEmailRequest) => {
        notifySuccess("Success!");
        Auth.confirmEmail(request)
            .then((response) => {
                navigate("/");
            })
            .catch((error) => {
                notifyError("Error during confirming email");
            });
    }

    const [userId, setUserId] = React.useState<string | undefined>(undefined);
    const [activeStep, setActiveStep] = React.useState(0);

    useEffect(() => {
        const fetchData = async () => {
            const user = await Auth.me();
            if (user)
                updateUser(user);
        }

        if (userId) {
            fetchData()
                .then(() => {
                    setActiveStep(1);
                });
        }
    }, [updateUser, userId]);

    const steps = [
        {
            label: 'Sign Up',
            description: 'Enter your personal information',
            content: (
                <SignUpForm onSubmit={(fields) => signUp(fields)} />
            ),
        },
        {
            label: 'Email confirmation',
            description: 'Enter the confirmation code sent to your email',
            content: (
                <EmailConfirmForm
                    userId={userId!}
                    onSubmit={(confirmationNumber) => {
                        return confirmEmail({ code: confirmationNumber, userId: userId! });
                    }} />
            ),
        },
    ];

    return (
        <>
            <Paper
                elevation={3}
                className="sign-up-paper">
                <Box>
                    <Box className="sign-up-header">
                        <Typography
                            className="sign-up-title"
                            variant='h4'>
                            {steps[activeStep].label}
                        </Typography>
                        <Typography
                            className="sign-up-description"
                            variant='subtitle1'>
                            {steps[activeStep].description}
                        </Typography>
                    </Box>
                    <Box
                        key={steps[activeStep].label}
                        sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            alignItems: 'center',
                            textAlign: 'center'
                        }}>
                        {steps[activeStep].content}
                    </Box>
                </Box>
                <Box sx={{
                    width: '100%',
                    position: 'absolute',
                    bottom: '15px',
                }}>
                    <Stepper
                        alternativeLabel
                        activeStep={activeStep}
                        orientation='horizontal'>
                        <Step>
                            <StepLabel>Info</StepLabel>
                        </Step>
                        <Step>
                            <StepLabel>Email confirmation</StepLabel>
                        </Step>
                    </Stepper>
                </Box>
            </Paper >
            <Notification />
        </>
    );
};

export default SignUpPage;