import React, { useState } from 'react';
import Snackbar from '@mui/material/Snackbar';
import Alert, { AlertColor } from '@mui/material/Alert';

interface NotificationProps {
    open: boolean;
    message: string;
    severity: AlertColor;
    onClose: () => void;
}

const Notification: React.FC<NotificationProps> = ({ open, message, severity, onClose }) => (
    <Snackbar open={open} autoHideDuration={6000} onClose={onClose}>
        <Alert onClose={onClose} severity={severity} sx={{ width: '100%' }}>
            {message}
        </Alert>
    </Snackbar>
);

interface UseNotification {
    notifySuccess: (message: string) => void;
    notifyError: (message: string) => void;
    notifyInfo: (message: string) => void;
    notifyWarning: (message: string) => void;
    Notification: React.FC;
}

const useNotification = (): UseNotification => {
    const [open, setOpen] = useState(false);
    const [message, setMessage] = useState('');
    const [severity, setSeverity] = useState<AlertColor>('success');

    const handleClose = () => {
        setOpen(false);
    };

    const notify = (message: string, severity: AlertColor) => {
        setMessage(message);
        setSeverity(severity);
        setOpen(true);
    };

    const notifySuccess = (message: string) => {
        notify(message, 'success');
    };

    const notifyError = (message: string) => {
        notify(message, 'error');
    };

    const notifyInfo = (message: string) => {
        notify(message, 'info');
    };

    const notifyWarning = (message: string) => {
        notify(message, 'warning');
    };

    return {
        notifyWarning,
        notifyInfo,
        notifySuccess,
        notifyError,
        Notification: () => <Notification open={open} message={message} severity={severity} onClose={handleClose} />
    };
};

export default useNotification;
