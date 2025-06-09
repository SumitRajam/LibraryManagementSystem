import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import {
    Box, Heading, Table, Thead, Tbody, Tr, Th, Td, Badge, useToast
} from '@chakra-ui/react';
import axiosInstance from '../../api/axiosInstance';

interface Borrow {
    id: number;
    bookId: number;
    bookTitle: string;
    borrowDate: string;
    dueDate: string;
    returnDate: string | null;
    status: string;
}

const MemberBorrows = () => {
    const { userId } = useParams<{ userId: string }>();
    const [borrows, setBorrows] = useState<Borrow[]>([]);
    const toast = useToast();

    useEffect(() => {
        if (userId) {
            fetchMemberBorrows(userId);
        }
    }, [userId]);

    const fetchMemberBorrows = async (userId: string) => {
        try {
            const response = await axiosInstance.get(`/Borrow/user/${userId}`);
            setBorrows(response.data);
        } catch (error) {
            toast({
                title: 'Error fetching member borrows',
                status: 'error',
                duration: 5000,
                isClosable: true,
            });
        }
    };

    const getStatusBadge = (status: string) => {
        switch (status.toLowerCase()) {
            case 'overdue':
                return <Badge colorScheme="red">Overdue</Badge>;
            case 'pending':
                return <Badge colorScheme="yellow">Pending</Badge>;
            case 'returned':
                return <Badge colorScheme="green">Returned</Badge>;
            default:
                return <Badge>{status}</Badge>;
        }
    };

    return (
        <Box p={4}>
            <Heading size="lg" mb={4}>Member's Borrowed Books</Heading>

            <Table variant="simple">
                <Thead>
                    <Tr>
                        <Th>Book</Th>
                        <Th>Borrow Date</Th>
                        <Th>Due Date</Th>
                        <Th>Return Date</Th>
                        <Th>Status</Th>
                    </Tr>
                </Thead>
                <Tbody>
                    {borrows.map(borrow => (
                        <Tr key={borrow.id}>
                            <Td>{borrow.bookTitle}</Td>
                            <Td>{new Date(borrow.borrowDate).toLocaleDateString()}</Td>
                            <Td>{new Date(borrow.dueDate).toLocaleDateString()}</Td>
                            <Td>{borrow.returnDate ? new Date(borrow.returnDate).toLocaleDateString() : 'Not returned'}</Td>
                            <Td>{getStatusBadge(borrow.status)}</Td>
                        </Tr>
                    ))}
                </Tbody>
            </Table>
        </Box>
    );
};

export default MemberBorrows;