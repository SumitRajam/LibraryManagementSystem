import React from 'react';
import { Outlet } from 'react-router-dom';
import Navbar from '../components/Navbar';
import { Box } from '@chakra-ui/react';

const AdminDashboard = () => {
    return (
        <Box>
            <Navbar />
            <Box p={4}>
                <Outlet />
            </Box>
        </Box>
    );
};

export default AdminDashboard;