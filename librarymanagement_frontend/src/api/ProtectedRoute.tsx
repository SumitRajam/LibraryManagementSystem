import { Navigate, Outlet } from 'react-router-dom';
import { authStore } from '../stores/authStore';

interface ProtectedRouteProps {
    requiredRole?: string;
}

export const ProtectedRoute = ({ requiredRole }: ProtectedRouteProps) => {
    const isAuthenticated = authStore(state => state.isAuthenticated());
    const hasRole = authStore(state => requiredRole ? state.hasRole(requiredRole) : true);

    if (!isAuthenticated) {
        return <Navigate to="/login" replace />;
    }

    if (requiredRole && !hasRole) {
        return <Navigate to="/unauthorized" replace />;
    }

    return <Outlet />;
};