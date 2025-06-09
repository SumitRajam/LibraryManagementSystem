import React from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import './App.css';
import Register from './pages/Register';
import Login from './pages/Login';
import { ProtectedRoute } from './api/ProtectedRoute';
import AdminDashboard from './pages/AdminDashboard';
import LibrarianDashboard from './pages/LibrarianDashboard';
import AdminBooks from './pages/admin/AdminBooks';
import AdminMembers from './pages/admin/AdminMembers';
import LibrarianHome from './pages/librarian/LibrarianHome';
import AssignBooks from './pages/librarian/AssignBooks';
import PendingReturns from './pages/librarian/PendingReturns';
import MemberBorrows from './pages/librarian/MemberBorrows';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/unauthorized" element={<p>Unauthorised User</p>} />

        <Route element={<ProtectedRoute />}>
          <Route path="/" element={<Navigate to="/login" replace />} />
          <Route element={<ProtectedRoute requiredRole="Admin" />}>
            <Route path="/admin/dashboard" element={<AdminDashboard />}>
              <Route index element={<h2>Admin Home</h2>} />
              <Route path="books" element={<AdminBooks />} />
              <Route path="members" element={<AdminMembers />} />
            </Route>
          </Route>

          <Route element={<ProtectedRoute requiredRole="Librarian" />}>
            <Route path="/librarian/dashboard" element={<LibrarianDashboard />}>
              <Route index element={<LibrarianHome />} />
              <Route path="assign-books" element={<AssignBooks />} />
              <Route path="pending-returns" element={<PendingReturns />} />
              <Route path="member-borrows/:userId" element={<MemberBorrows />} />
            </Route>
          </Route>
        </Route>

        <Route path="*" element={<Navigate to="/login" replace />} />
      </Routes>
    </Router>
  );
}

export default App;