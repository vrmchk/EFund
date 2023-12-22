import { Box, Button, TextField } from "@mui/material";
import { useForm } from "react-hook-form";

interface EmailConfirmFormProps {
    userId: string;
    onSubmit: (confirmationNumber: number) => void;
}

interface EmailConfirmFormFields {
    confirmationNumber: number;
}

const EmailConfirmForm = (props: EmailConfirmFormProps) => {
    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm<EmailConfirmFormFields>({
        reValidateMode: 'onChange',
        mode: 'onTouched'
    });
    return (
        <Box width='500px'>
            <Box
                display={'flex'}
                flexDirection={'column'}
                sx={{ gap: 3, margin: 7, mt: 5 }}
                component="form"
                onSubmit={handleSubmit((fields) => props.onSubmit(fields.confirmationNumber))}>
                <TextField
                    {...register("confirmationNumber")}
                    label="Confirmation Number"
                    variant='standard'
                    autoComplete="off"
                    fullWidth
                    error={!!errors.confirmationNumber}
                />
                <Button
                    type="submit"
                    variant="contained"
                    color="primary"
                    size="large"
                    sx={{ width: 'max-content', alignSelf: 'center' }}>
                    Confirm
                </Button>
            </Box>
        </Box>
    );
};

export default EmailConfirmForm;