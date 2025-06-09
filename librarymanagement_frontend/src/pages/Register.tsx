import { useMutation } from '@tanstack/react-query';
import { useForm } from 'react-hook-form';
import { Link, useNavigate } from 'react-router-dom';
import { Box, Button, FormControl, FormErrorMessage, FormLabel, Heading, Input, Stack, Text, useToast, } from '@chakra-ui/react';
import axiosInstance from '../api/axiosInstance';

interface RegisterFormData {
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    confirmPassword: string;
}

export default function Register() {
    const toast = useToast();
    const navigate = useNavigate();

    const {
        register,
        handleSubmit,
        watch,
        formState: { errors },
    } = useForm<RegisterFormData>();

    const { mutate, isPending } = useMutation({
        mutationFn: (data: Omit<RegisterFormData, 'confirmPassword'>) =>
            axiosInstance.post('/Auth/register', data),
        onSuccess: () => {
            toast({
                title: 'Registration successful',
                status: 'success',
                duration: 3000,
            });
            navigate('/login');
        },
        onError: (error: any) => {
            toast({
                title: 'Registration failed',
                description: error.response?.data?.message || 'Unknown error occurred',
                status: 'error',
                duration: 5000,
            });
        },
    });

    const onSubmit = (data: RegisterFormData) => {
        const { confirmPassword, ...registrationData } = data;
        mutate(registrationData);
    };

    const password = watch('password');

    return (
        <Box maxW="md" mx="auto" mt={10} p={6} borderWidth={1} borderRadius="md">
            <Heading as="h1" size="xl" mb={6} textAlign="center">
                Create Account
            </Heading>

            <form onSubmit={handleSubmit(onSubmit)}>
                <Stack spacing={4}>
                    <FormControl isInvalid={!!errors.firstName}>
                        <FormLabel>First Name</FormLabel>
                        <Input
                            type="text"
                            placeholder="John"
                            {...register('firstName', {
                                required: 'First name is required',
                                minLength: {
                                    value: 2,
                                    message: 'First name must be at least 2 characters',
                                },
                            })}
                        />
                        <FormErrorMessage>{errors.firstName?.message}</FormErrorMessage>
                    </FormControl>

                    <FormControl isInvalid={!!errors.lastName}>
                        <FormLabel>Last Name</FormLabel>
                        <Input
                            type="text"
                            placeholder="Doe"
                            {...register('lastName', {
                                required: 'Last name is required',
                                minLength: {
                                    value: 2,
                                    message: 'Last name must be at least 2 characters',
                                },
                            })}
                        />
                        <FormErrorMessage>{errors.lastName?.message}</FormErrorMessage>
                    </FormControl>

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
                            placeholder="••••••••"
                            {...register('password', {
                                required: 'Password is required',
                                minLength: {
                                    value: 8,
                                    message: 'Password must be at least 8 characters',
                                },
                                pattern: {
                                    value: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/,
                                    message: 'Password must contain at least one uppercase, one lowercase, one number and one special character',
                                },
                            })}
                        />
                        <FormErrorMessage>{errors.password?.message}</FormErrorMessage>
                    </FormControl>

                    <FormControl isInvalid={!!errors.confirmPassword}>
                        <FormLabel>Confirm Password</FormLabel>
                        <Input
                            type="password"
                            placeholder="••••••••"
                            {...register('confirmPassword', {
                                required: 'Please confirm your password',
                                validate: (value) =>
                                    value === password || 'Passwords do not match',
                            })}
                        />
                        <FormErrorMessage>{errors.confirmPassword?.message}</FormErrorMessage>
                    </FormControl>

                    <Button
                        type="submit"
                        colorScheme="blue"
                        isLoading={isPending}
                        loadingText="Registering..."
                        mt={4}
                    >
                        Register
                    </Button>
                </Stack>
            </form>

            <Text mt={4} textAlign="center">
                Already have an account?{' '}
                <Link to="/login" style={{ color: '#3182ce', fontWeight: 'bold' }}>
                    Sign in
                </Link>
            </Text>
        </Box>
    );
}