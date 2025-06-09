import { Outlet } from 'react-router-dom';
import Navbar from '../components/Navbar';
import { Box } from '@chakra-ui/react';

const LibrarianDashboard = () => {
    return (
        <Box>
            <Navbar />
            <Box p={4}>
                <Outlet />
            </Box>
        </Box>
    );
};

export default LibrarianDashboard;