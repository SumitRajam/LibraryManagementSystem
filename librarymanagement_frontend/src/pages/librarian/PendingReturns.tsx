import React, { useState, useEffect } from 'react';
import { Box, Table, Thead, Tbody, Tr, Th, Td, Button, useToast, Badge } from '@chakra-ui/react';
import axiosInstance from '../../api/axiosInstance';

interface Borrow {
    id: number;
    bookId: number;
    bookTitle: string;
    userId: string;
    userName: string;
    borrowDate: string;
    dueDate: string;
    returnDate: string | null;
    status: string;
}

const PendingReturns = () => {
    const [pendingBorrows, setPendingBorrows] = useState<Borrow[]>([]);
    const toast = useToast();

    useEffect(() => {
        fetchPendingReturns();
    }, []);

    const fetchPendingReturns = async () => {
        try {
            const response = await axiosInstance.get('/Borrow/pending');
            setPendingBorrows(response.data);
        } catch (error) {
            toast({
                title: 'Error fetching pending returns',
                status: 'error',
                duration: 5000,
                isClosable: true,
            });
        }
    };

    const handleReturn = async (borrowId: number) => {
        try {
            await axiosInstance.put(`/Borrow/${borrowId}/return`);
            toast({
                title: 'Book returned successfully',
                status: 'success',
                duration: 3000,
                isClosable: true,
            });
            fetchPendingReturns();
        } catch (error) {
            toast({
                title: 'Error returning book',
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
            <Table variant="simple">
                <Thead>
                    <Tr>
                        <Th>Book</Th>
                        <Th>Member</Th>
                        <Th>Borrow Date</Th>
                        <Th>Due Date</Th>
                        <Th>Status</Th>
                        <Th>Action</Th>
                    </Tr>
                </Thead>
                <Tbody>
                    {pendingBorrows.map(borrow => (
                        <Tr key={borrow.id}>
                            <Td>{borrow.bookTitle}</Td>
                            <Td>{borrow.userName}</Td>
                            <Td>{new Date(borrow.borrowDate).toLocaleDateString()}</Td>
                            <Td>{new Date(borrow.dueDate).toLocaleDateString()}</Td>
                            <Td>{getStatusBadge(borrow.status)}</Td>
                            <Td>
                                {borrow.status.toLowerCase() !== 'returned' && (
                                    <Button
                                        colorScheme="green"
                                        size="sm"
                                        onClick={() => handleReturn(borrow.id)}
                                    >
                                        Mark Returned
                                    </Button>
                                )}
                            </Td>
                        </Tr>
                    ))}
                </Tbody>
            </Table>
        </Box>
    );
};

export default PendingReturns;