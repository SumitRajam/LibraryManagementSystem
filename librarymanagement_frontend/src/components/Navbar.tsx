import React from 'react';
import { Link, useLocation } from 'react-router-dom';
import { Flex, Box, Button, Text, useColorModeValue } from '@chakra-ui/react';
import { authStore } from '../stores/authStore';

const Navbar = () => {
    const location = useLocation();
    const { logout, role } = authStore();
    const bg = useColorModeValue('gray.100', 'gray.900');

    const getAdminLinks = () => (
        <>
            <Button
                as={Link}
                to="/admin/dashboard/books"
                variant="ghost"
                isActive={location.pathname.includes('/admin/dashboard/books')}
            >
                Books
            </Button>
            <Button
                as={Link}
                to="/admin/dashboard/members"
                variant="ghost"
                isActive={location.pathname.includes('/admin/dashboard/members')}
            >
                Members
            </Button>
        </>
    );

    const getLibrarianLinks = () => (
        <>
            <Button
                as={Link}
                to="/librarian/dashboard/assign-books"
                variant={location.pathname.includes('assign-books') ? 'solid' : 'ghost'}
            >
                Assign Books
            </Button>
            <Button
                as={Link}
                to="/librarian/dashboard/pending-returns"
                variant={location.pathname.includes('pending-returns') ? 'solid' : 'ghost'}
                ml={2}
            >
                Pending Returns
            </Button>
        </>
    );

    const getMemberLinks = () => (
        <Button
            as={Link}
            to="/member/dashboard/borrowed"
            variant="ghost"
            isActive={location.pathname.includes('/member/dashboard/borrowed')}
        >
            My Books
        </Button>
    );

    return (
        <Box bg={bg} px={4} py={2}>
            <div style={{ width: '100%', maxWidth: '1200px', margin: '0 auto' }}>
                <Flex alignItems="center" justifyContent="space-between">
                    <Flex>
                        {role === 'Admin' && getAdminLinks()}
                        {role === 'Librarian' && getLibrarianLinks()}
                    </Flex>
                    <Flex alignItems="center">
                        <Text mr={4}>Welcome, {authStore.getState().name}</Text>
                        <Button colorScheme="red" onClick={logout}>
                            Logout
                        </Button>
                    </Flex>
                </Flex>
            </div>
        </Box>
    );
};

export default Navbar;