import { useState, useEffect } from 'react';
import User from '../models/user/User';
import Auth from '../services/api/auth/Auth';

const useUser = () => {
    const [user, setUser] = useState<User | null>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        setLoading(true);

        const fetchUser = async () => {
            try {
                const user = await Auth.me();
                if (user) {
                    setUser(user);
                }
            } catch (error) {
                console.log("Error logout:", error);
                logout();
            }
            finally {
                setLoading(false);
            }
        };

        fetchUser();
    }, []);

    const updateUser = (newUser: User) => {
        setUser(newUser);
    };

    const logout = () => {
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
        setUser(null);
    };

    return { user, loading, updateUser, logout };
};

export default useUser;
