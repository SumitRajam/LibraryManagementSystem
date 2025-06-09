import React, { useState, useEffect } from 'react';
import { Table, Thead, Tbody, Tr, Th, Td, Box, Button, useDisclosure, Modal, ModalOverlay, ModalContent, ModalHeader, ModalFooter, ModalBody, ModalCloseButton, FormControl, FormLabel, Input, useToast, IconButton, Flex, Heading } from '@chakra-ui/react';
import { EditIcon, DeleteIcon, AddIcon } from '@chakra-ui/icons';
import axiosInstance from '../../api/axiosInstance';

interface Book {
    id: number;
    title: string;
    author: string;
    isbn: string;
    publishedYear: number;
    availableCopies: number;
    totalCopies: number;
}

const AdminBooks = () => {
    const [books, setBooks] = useState<Book[]>([]);
    const [selectedBook, setSelectedBook] = useState<Book | null>(null);
    const [isEditMode, setIsEditMode] = useState(false);
    const { isOpen, onOpen, onClose } = useDisclosure();
    const toast = useToast();

    const initialFormData = {
        title: '',
        author: '',
        isbn: '',
        publishedYear: new Date().getFullYear(),
        totalCopies: 1
    };

    const [formData, setFormData] = useState(initialFormData);

    useEffect(() => {
        fetchBooks();
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

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: name === 'publishedYear' || name === 'totalCopies' ? parseInt(value) : value,
        });
    };

    const handleSubmit = async () => {
        try {
            if (isEditMode && selectedBook) {
                await axiosInstance.put(`/Books/${selectedBook.id}`, {
                    id: selectedBook.id,
                    ...formData
                });
                toast({
                    title: 'Book updated successfully',
                    status: 'success',
                    duration: 3000,
                    isClosable: true,
                });
            } else {
                await axiosInstance.post('/Books', formData);
                toast({
                    title: 'Book created successfully',
                    status: 'success',
                    duration: 3000,
                    isClosable: true,
                });
            }
            fetchBooks();
            onClose();
            setFormData(initialFormData);
        } catch (error) {
            toast({
                title: 'Error saving book',
                status: 'error',
                duration: 5000,
                isClosable: true,
            });
        }
    };

    const handleEdit = (book: Book) => {
        setSelectedBook(book);
        setIsEditMode(true);
        setFormData({
            title: book.title,
            author: book.author,
            isbn: book.isbn,
            publishedYear: book.publishedYear,
            totalCopies: book.totalCopies
        });
        onOpen();
    };

    const handleDelete = async (id: number) => {
        try {
            await axiosInstance.delete(`/Books/${id}`);
            toast({
                title: 'Book deleted successfully',
                status: 'success',
                duration: 3000,
                isClosable: true,
            });
            fetchBooks();
        } catch (error) {
            toast({
                title: 'Error deleting book',
                status: 'error',
                duration: 5000,
                isClosable: true,
            });
        }
    };

    const handleAddNew = () => {
        setSelectedBook(null);
        setIsEditMode(false);
        setFormData(initialFormData);
        onOpen();
    };

    return (
        <Box p={4}>
            <Flex justifyContent="space-between" alignItems="center" mb={4}>
                <Heading size="lg">Books Management</Heading>
                <Button leftIcon={<AddIcon />} colorScheme="teal" onClick={handleAddNew}>
                    Add Book
                </Button>
            </Flex>

            <Table variant="simple">
                <Thead>
                    <Tr>
                        <Th>Title</Th>
                        <Th>Author</Th>
                        <Th>ISBN</Th>
                        <Th>Year</Th>
                        <Th>Available</Th>
                        <Th>Total</Th>
                        <Th>Actions</Th>
                    </Tr>
                </Thead>
                <Tbody>
                    {books.map((book) => (
                        <Tr key={book.id}>
                            <Td>{book.title}</Td>
                            <Td>{book.author}</Td>
                            <Td>{book.isbn}</Td>
                            <Td>{book.publishedYear}</Td>
                            <Td>{book.availableCopies}</Td>
                            <Td>{book.totalCopies}</Td>
                            <Td>
                                <IconButton
                                    aria-label="Edit book"
                                    icon={<EditIcon />}
                                    mr={2}
                                    onClick={() => handleEdit(book)}
                                />
                                <IconButton
                                    aria-label="Delete book"
                                    icon={<DeleteIcon />}
                                    colorScheme="red"
                                    onClick={() => handleDelete(book.id)}
                                />
                            </Td>
                        </Tr>
                    ))}
                </Tbody>
            </Table>

            <Modal isOpen={isOpen} onClose={onClose}>
                <ModalOverlay />
                <ModalContent>
                    <ModalHeader>{isEditMode ? 'Edit Book' : 'Add New Book'}</ModalHeader>
                    <ModalCloseButton />
                    <ModalBody pb={6}>
                        <FormControl>
                            <FormLabel>Title</FormLabel>
                            <Input
                                name="title"
                                value={formData.title}
                                onChange={handleInputChange}
                                placeholder="Book title"
                            />
                        </FormControl>

                        <FormControl mt={4}>
                            <FormLabel>Author</FormLabel>
                            <Input
                                name="author"
                                value={formData.author}
                                onChange={handleInputChange}
                                placeholder="Author name"
                            />
                        </FormControl>

                        {!isEditMode && (
                            <FormControl mt={4}>
                                <FormLabel>ISBN</FormLabel>
                                <Input
                                    name="isbn"
                                    value={formData.isbn}
                                    onChange={handleInputChange}
                                    placeholder="ISBN number"
                                />
                            </FormControl>
                        )}

                        <FormControl mt={4}>
                            <FormLabel>Published Year</FormLabel>
                            <Input
                                type="number"
                                name="publishedYear"
                                value={formData.publishedYear}
                                onChange={handleInputChange}
                            />
                        </FormControl>

                        <FormControl mt={4}>
                            <FormLabel>Total Copies</FormLabel>
                            <Input
                                type="number"
                                name="totalCopies"
                                value={formData.totalCopies}
                                onChange={handleInputChange}
                            />
                        </FormControl>
                    </ModalBody>

                    <ModalFooter>
                        <Button colorScheme="blue" mr={3} onClick={handleSubmit}>
                            Save
                        </Button>
                        <Button onClick={onClose}>Cancel</Button>
                    </ModalFooter>
                </ModalContent>
            </Modal>
        </Box>
    );
};

export default AdminBooks;