import axios from 'axios';
import { authStore } from '../stores/authStore';

const baseURL = 'https://localhost:7181/api';

const axiosInstance = axios.create({
    baseURL,
    headers: {
        'Content-Type': 'application/json',
    },
});

axiosInstance.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem('token');
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

axiosInstance.interceptors.response.use(
    (response) => response,
    (error) => {
        if (error.response?.status === 401) {
            authStore.getState().logout();
        }
        return Promise.reject(error);
    }
);

export default axiosInstance;