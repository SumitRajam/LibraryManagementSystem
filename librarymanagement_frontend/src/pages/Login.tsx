import { useMutation } from '@tanstack/react-query';
import { useForm } from 'react-hook-form';
import { Link, useNavigate } from 'react-router-dom';
import { Box, Button, FormControl, FormErrorMessage, FormLabel, Heading, Input, Stack, Text, useToast, } from '@chakra-ui/react';
import { authStore } from '../stores/authStore';

interface LoginFormData {
    email: string;
    password: string;
}

export default function Login() {
    const toast = useToast();
    const navigate = useNavigate();
    const login = authStore((state) => state.login);

    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm<LoginFormData>();

    const { mutate, isPending } = useMutation({
        mutationFn: (data: LoginFormData) => login(data.email, data.password),
        onSuccess: () => {
            toast({
                title: 'Login successful',
                status: 'success',
                duration: 3000,
            });
            const role = authStore.getState().role;
            switch (role) {
                case 'Admin':
                    navigate('/admin/dashboard');
                    break;
                case 'Librarian':
                    navigate('/librarian/dashboard');
                    break;
                case 'Member':
                    navigate('/member/dashboard');
                    break;
                default:
                    navigate('/dashboard');
            }
        },
        onError: (error: Error) => {
            toast({
                title: 'Login failed',
                description: error.message || 'Invalid credentials',
                status: 'error',
                duration: 5000,
            });
        },
    });

    const onSubmit = (data: LoginFormData) => {
        mutate(data);
    };

    return (
        <Box maxW="md" mx="auto" mt={10} p={6} borderWidth={1} borderRadius="md">
            <Heading as="h1" size="xl" mb={6} textAlign="center">
                Welcome Back
            </Heading>

            <form onSubmit={handleSubmit(onSubmit)}>
                <Stack spacing={4}>
                    <FormControl isInvalid={!!errors.email}>
                        <FormLabel>Email Address</FormLabel>
                        <Input
                            type="email"
                            placeholder="your@email.com"
                            {...register('email', {
                                required: 'Email is required',
                                pattern: {
                                    value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
                                    message: 'Invalid email address',
                                },
                            })}
                        />
                        <FormErrorMessage>{errors.email?.message}</FormErrorMessage>
                    </FormControl>

                    <FormControl isInvalid={!!errors.password}>
                        <FormLabel>Password</FormLabel>
                        <Input
                            type="password"
                            placeholder="********"
                            {...register('password', {
                                required: 'Password is required',
                            })}
                        />
                        <FormErrorMessage>{errors.password?.message}</FormErrorMessage>
                    </FormControl>

                    <Button
                        type="submit"
                        colorScheme="blue"
                        isLoading={isPending}
                        loadingText="Signing in..."
                        mt={4}
                    >
                        Sign In
                    </Button>
                </Stack>
            </form>

            <Text mt={4} textAlign="center">
                Don't have an account?{' '}
                <Link to="/register" style={{ color: '#3182ce', fontWeight: 'bold' }}>
                    Register
                </Link>
            </Text>
        </Box>
    );
}