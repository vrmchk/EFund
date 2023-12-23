
import { Box, Button, TextField } from "@mui/material";
import { useForm } from "react-hook-form";
import { SignInFormFields } from "../../../models/form/auth/AuthFormFields";

interface SignInFormProps {
    onSubmit: (data: SignInFormFields) => void;
}

const SignInForm = (props: SignInFormProps) => {
    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm<SignInFormFields>();

    return (
        <Box width='400px'>
            <Box
                display={'flex'}
                flexDirection={'column'}
                sx={{ gap: 3, margin: 5, mt: 10, mb: 10 }}
                component="form"
                onSubmit={handleSubmit(props.onSubmit)}>
                <TextField
                    id="email"
                    label="Email"
                    {...register('email')}
                    error={!!errors.email}
                    helperText={errors.email?.message}
                    variant="standard"
                    autoComplete='off'
                />
                <TextField
                    id="password"
                    type="password"
                    label="Password"
                    {...register('password')}
                    error={!!errors.password}
                    helperText={errors.password?.message}
                    variant="standard"
                />
                <Button
                    type="submit"
                    variant="contained"
                    color="primary"
                    size="large"
                    sx={{ width: 'max-content', alignSelf: 'center' }}>
                    Sign In
                </Button>
            </Box>
        </Box>
    );
};

export default SignInForm;