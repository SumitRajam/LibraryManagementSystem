import React, { useState, useEffect } from 'react';
import { Box, Button, Flex, Heading, Table, Thead, Tbody, Tr, Th, Td, useDisclosure, Modal, ModalOverlay, ModalContent, ModalHeader, ModalCloseButton, ModalBody, ModalFooter, FormControl, FormLabel, Input, Select, useToast } from '@chakra-ui/react';
import axiosInstance from '../../api/axiosInstance';

interface Book {
    id: number;
    title: string;
    author: string;
    availableCopies: number;
}

interface Member {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
}

const AssignBooks = () => {
    const [books, setBooks] = useState<Book[]>([]);
    const [members, setMembers] = useState<Member[]>([]);
    const [selectedBook, setSelectedBook] = useState<number | null>(null);
    const [selectedMember, setSelectedMember] = useState<string | null>(null);
    const [daysToBorrow, setDaysToBorrow] = useState<number>(14);
    const { isOpen, onOpen, onClose } = useDisclosure();
    const toast = useToast();

    useEffect(() => {
        fetchBooks();
        fetchMembers();
    }, []);

    const fetchBooks = async () => {
        try {
            const response = await axiosInstance.get('/Books');
            setBooks(response.data);
        } catch (error) {
            toast({
                title: 'Error fetching books',
                status: 'error',
                duration: 5000,
                isClosable: true,
            });
        }
    };

    const fetchMembers = async () => {
        try {
            const response = await axiosInstance.get('/Roles/ListMembers');
            setMembers(response.data);
        } catch (error) {
            toast({
                title: 'Error fetching members',
                status: 'error',
                duration: 5000,
                isClosable: true,
            });
        }
    };

    const handleAssign = async () => {
        if (!selectedBook || !selectedMember) return;

        try {
            await axiosInstance.post('/Borrow', {
                bookId: selectedBook,
                userId: selectedMember,
                daysToBorrow: daysToBorrow
            });

            toast({
                title: 'Book assigned successfully',
                status: 'success',
                duration: 3000,
                isClosable: true,
            });

            // Refresh available copies
            fetchBooks();
            onClose();
        } catch (error) {
            toast({
                title: 'Error assigning book',
                status: 'error',
                duration: 5000,
                isClosable: true,
            });
        }
    };

    return (
        <Box p={4}>
            <Flex justifyContent="space-between" alignItems="center" mb={4}>
                <Heading size="lg">Assign Books to Members</Heading>
                <Button colorScheme="blue" onClick={onOpen}>Assign Book</Button>
            </Flex>

            <Table variant="simple">
                <Thead>
                    <Tr>
                        <Th>Title</Th>
                        <Th>Author</Th>
                        <Th>Available Copies</Th>
                    </Tr>
                </Thead>
                <Tbody>
                    {books.map(book => (
                        <Tr key={book.id}>
                            <Td>{book.title}</Td>
                            <Td>{book.author}</Td>
                            <Td>{book.availableCopies}</Td>
                        </Tr>
                    ))}
                </Tbody>
            </Table>

            <Modal isOpen={isOpen} onClose={onClose}>
                <ModalOverlay />
                <ModalContent>
                    <ModalHeader>Assign Book to Member</ModalHeader>
                    <ModalCloseButton />
                    <ModalBody>
                        <FormControl mb={4}>
                            <FormLabel>Select Book</FormLabel>
                            <Select
                                placeholder="Select book"
                                onChange={(e) => setSelectedBook(Number(e.target.value))}
                            >
                                {books.filter(b => b.availableCopies > 0).map(book => (
                                    <option key={book.id} value={book.id}>
                                        {book.title} by {book.author}
                                    </option>
                                ))}
                            </Select>
                        </FormControl>

                        <FormControl mb={4}>
                            <FormLabel>Select Member</FormLabel>
                            <Select
                                placeholder="Select member"
                                onChange={(e) => setSelectedMember(e.target.value)}
                            >
                                {members.map(member => (
                                    <option key={member.id} value={member.id}>
                                        {member.firstName} {member.lastName} ({member.email})
                                    </option>
                                ))}
                            </Select>
                        </FormControl>

                        <FormControl>
                            <FormLabel>Days to Borrow</FormLabel>
                            <Input
                                type="number"
                                value={daysToBorrow}
                                onChange={(e) => setDaysToBorrow(Number(e.target.value))}
                            />
                        </FormControl>
                    </ModalBody>

                    <ModalFooter>
                        <Button colorScheme="blue" mr={3} onClick={handleAssign}>
                            Assign
                        </Button>
                        <Button onClick={onClose}>Cancel</Button>
                    </ModalFooter>
                </ModalContent>
            </Modal>
        </Box>
    );
};

export default AssignBooks;