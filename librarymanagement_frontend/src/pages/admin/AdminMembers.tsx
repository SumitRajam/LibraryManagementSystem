import React, { useState, useEffect } from 'react';
import { Table, Thead, Tbody, Tr, Th, Td, Box, Button, useDisclosure, Modal, ModalOverlay, ModalContent, ModalHeader, ModalFooter, ModalBody, ModalCloseButton, FormControl, FormLabel, Select, useToast, Flex, Heading } from '@chakra-ui/react';
import axiosInstance from '../../api/axiosInstance';

interface Member {
    id: string;
    email: string;
    firstName: string;
    lastName: string;
    userName: string;
}

interface UserRoleDTO {
    roleName: string;
    isAssigned: boolean;
}

const AdminMembers = () => {
    const [membersWithRoles, setMembersWithRoles] = useState<{ member: Member; role: string }[]>([]);
    const [selectedMember, setSelectedMember] = useState<Member | null>(null);
    const [selectedRole, setSelectedRole] = useState('');
    const { isOpen, onOpen, onClose } = useDisclosure();
    const toast = useToast();

    useEffect(() => {
        const fetchMembersWithRoles = async () => {
            try {
                const response = await axiosInstance.get('/Roles/ListMembers');
                const members = response.data;

                // Fetch roles for all members
                const membersWithRoles = await Promise.all(
                    members.map(async (member: Member) => {
                        try {
                            const rolesResponse = await axiosInstance.get(`/RoleAssignment/user-roles/${member.id}`);
                            const assignedRole = rolesResponse.data.find((r: UserRoleDTO) => r.isAssigned);
                            return {
                                member,
                                role: assignedRole?.roleName || 'Member'
                            };
                        } catch (error) {
                            console.error(`Error fetching roles for member ${member.id}:`, error);
                            return {
                                member,
                                role: 'Member' // Fallback role
                            };
                        }
                    })
                );

                setMembersWithRoles(membersWithRoles);
            } catch (error) {
                toast({
                    title: 'Error fetching members',
                    status: 'error',
                    duration: 5000,
                    isClosable: true,
                });
            }
        };

        fetchMembersWithRoles();
    }, []);

    const handleRoleChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        setSelectedRole(e.target.value);
    };

    const handleAssignRole = async (member: Member) => {
        try {
            const response = await axiosInstance.get(`/RoleAssignment/user-roles/${member.id}`);
            const assignedRole = response.data.find((r: UserRoleDTO) => r.isAssigned);

            setSelectedMember(member);
            setSelectedRole(assignedRole?.roleName || 'Member');
            onOpen();
        } catch (error) {
            toast({
                title: 'Error fetching user roles',
                status: 'error',
                duration: 5000,
                isClosable: true,
            });
        }
    };

    const submitRoleAssignment = async () => {
        if (!selectedMember || !selectedRole) return;

        try {
            await axiosInstance.post('/RoleAssignment/assign', {
                userId: selectedMember.id,
                roleName: selectedRole
            });

            // Optimistic UI update
            setMembersWithRoles(prev =>
                prev.map(item =>
                    item.member.id === selectedMember.id
                        ? { ...item, role: selectedRole }
                        : item
                )
            );

            toast({
                title: 'Role assigned successfully',
                status: 'success',
                duration: 3000,
                isClosable: true,
            });
            onClose();
        } catch (error) {
            toast({
                title: 'Error assigning role',
                status: 'error',
                duration: 5000,
                isClosable: true,
            });
        }
    };

    return (
        <Box p={4}>
            <Flex justifyContent="space-between" alignItems="center" mb={4}>
                <Heading size="lg">Members Management</Heading>

            </Flex>

            <Table variant="simple">
                <Thead>
                    <Tr>
                        <Th>Name</Th>
                        <Th>Email</Th>
                        <Th>Role</Th>
                        <Th>Actions</Th>
                    </Tr>
                </Thead>
                <Tbody>
                    {membersWithRoles.map(({ member, role }) => (
                        <Tr key={member.id}>
                            <Td>{`${member.firstName} ${member.lastName}`}</Td>
                            <Td>{member.email}</Td>
                            <Td>{role}</Td>
                            <Td>
                                <Button
                                    colorScheme="teal"
                                    size="sm"
                                    onClick={() => handleAssignRole(member)}
                                >
                                    Change Role
                                </Button>
                            </Td>
                        </Tr>
                    ))}
                </Tbody>
            </Table>

            <Modal isOpen={isOpen} onClose={onClose}>
                <ModalOverlay />
                <ModalContent>
                    <ModalHeader>Change Role for {selectedMember?.firstName} {selectedMember?.lastName}</ModalHeader>
                    <ModalCloseButton />
                    <ModalBody pb={6}>
                        <FormControl>
                            <FormLabel>Select Role</FormLabel>
                            <Select
                                value={selectedRole}
                                onChange={handleRoleChange}
                                placeholder="Select role"
                            >
                                <option value="Admin">Admin</option>
                                <option value="Librarian">Librarian</option>
                                <option value="Member">Member</option>
                            </Select>
                        </FormControl>
                    </ModalBody>

                    <ModalFooter>
                        <Button colorScheme="blue" mr={3} onClick={submitRoleAssignment}>
                            Save
                        </Button>
                        <Button onClick={onClose}>Cancel</Button>
                    </ModalFooter>
                </ModalContent>
            </Modal>
        </Box>
    );
};

export default AdminMembers;