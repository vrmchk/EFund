import * as yup from 'yup';
import SignUpFormFields from '../../models/form/auth/SignUpFormFields';

const signUpFormValidation = yup.object<SignUpFormFields>().shape({
    name: yup.string()
        .required('Name is required')
        .trim('Name cannot contain leading and trailing spaces')
        .min(2, 'Name must be at least 2 characters')
        .max(50, 'Name must not exceed 50 characters'),
    email: yup.string()
        .required('Email is required')
        .email('Email is invalid'),
    password: yup.string()
        .required('Password is required')
        .min(8, 'Password must be at least 8 characters')
        .matches(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*#?&\\.,:;])[A-Za-z\d@$!%*#?&\\.,:;]+$/,
            'Password must contain at least one uppercase letter, one lowercase letter, one digit and one special character'),
    confirmPassword: yup.string()
        .required('Confirm Password is required')
        .oneOf([yup.ref('password')], "Password didn't match")
});

export default signUpFormValidation;