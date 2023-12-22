import React, { useEffect } from "react";
import Auth from "../services/api/auth/Auth";
import SignUpForm from "../components/auth/sign-up/SignUpForm";
import { ConfirmEmailRequest, SignUpRequest } from '../models/api/request/AuthRequests';
import Stepper from '@mui/material/Stepper';
import { Box, Paper, Step, StepLabel, Typography } from "@mui/material";
import EmailConfirmForm from "../components/auth/sign-up/EmailConfirmForm";
import { redirect } from "react-router-dom";
import '../styles/sign-up.css';
import useNotification from "../hooks/useNotification";

const SignUpPage = () => {

    const { notifySuccess, Notification } = useNotification();

    const signUp = async (request: SignUpRequest) => {
        setUserId("123");
        notifySuccess("Success!");
        // Auth.signUp(request)
        //     .then((response) => {
        //         setUserId(response.userId);
        //     })
        //     .catch((error) => {
        //         console.error("Error during signUp:", error);
        //     });
    }

    const confirmEmail = async (request: ConfirmEmailRequest) => {
        notifySuccess("Success!");
        // Auth.confirmEmail(request)
        //     .then((response) => {
        //         console.log("Email confirmed:", response);
        //         localStorage.setItem("accessToken", response.accessToken);
        //         localStorage.setItem("refreshToken", response.refreshToken);
        //         redirect("/");
        //     })
        //     .catch((error) => {
        //         console.error("Error during confirmEmail:", error);
        //     });
    }

    const [userId, setUserId] = React.useState<string | undefined>(undefined);
    const [activeStep, setActiveStep] = React.useState(0);

    useEffect(() => {
        if (userId) {
            console.log("userId:", userId);
            setActiveStep(1)
        }
    }, [userId]);

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