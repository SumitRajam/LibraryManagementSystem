import { create } from 'zustand';
import { persist } from 'zustand/middleware';
import axiosInstance from '../api/axiosInstance';

interface AuthState {
    token: string | null;
    role: string | null;
    userId: string | null;
    name: string | null;
    email: string | null;
    login: (email: string, password: string) => Promise<void>;
    logout: () => void;
    isAuthenticated: () => boolean;
    hasRole: (requiredRole: string) => boolean;
}

export const authStore = create<AuthState>()(
    persist(
        (set, get) => ({
            token: null,
            role: null,
            userId: null,
            name: null,
            email: null,

            login: async (email, password) => {
                try {
                    const response = await axiosInstance.post('/Auth/login', {
                        email,
                        password,
                    });

                    const token = response.data.token;
                    // You might want to decode the JWT to get user info or get it from response
                    const decodedToken = parseJwt(token);

                    set({
                        token,
                        role: decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'],
                        userId: decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'],
                        name: decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
                        email: decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'],
                    });

                    localStorage.setItem('token', token);
                } catch (error) {
                    throw error;
                }
            },

            logout: () => {
                set({
                    token: null,
                    role: null,
                    userId: null,
                    name: null,
                    email: null,
                });
                localStorage.removeItem('token');
            },

            isAuthenticated: () => {
                return !!get().token;
            },

            hasRole: (requiredRole) => {
                const { role } = get();
                if (!role) return false;
                return role === requiredRole;
            },
        }),
        {
            name: 'auth-storage',
        }
    )
);

function parseJwt(token: string) {
    try {
        const base64Url = token.split('.')[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const jsonPayload = decodeURIComponent(
            window.atob(base64)
                .split('')
                .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
                .join('')
        );
        return JSON.parse(jsonPayload);
    } catch (e) {
        return null;
    }
}