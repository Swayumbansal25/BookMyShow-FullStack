import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { userService } from '../api/userService';

const RegisterPage = () => {
    const [formData, setFormData] = useState({ fullName: '', email: '', passwordHash: '', phoneNumber: '' });
    const navigate = useNavigate();

    const handleRegister = async (e) => {
        e.preventDefault();
        try {
            // This calls your POST /Users API
            await userService.register(formData); 
            alert("Registration successful!");
            navigate('/login');
        } catch (error) {
            console.error("Error during registration:", error);
        }
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-gray-100">
            <form onSubmit={handleRegister} className="bg-white p-8 rounded shadow-md w-96 space-y-4">
                <h2 className="text-2xl font-bold text-center text-bmsPrimary">Create Account</h2>
                <input type="text" placeholder="Full Name" className="w-full p-2 border rounded" 
                    onChange={(e) => setFormData({...formData, fullName: e.target.value})} required />
                <input type="email" placeholder="Email" className="w-full p-2 border rounded" 
                    onChange={(e) => setFormData({...formData, email: e.target.value})} required />
                <input type="password" placeholder="Password" className="w-full p-2 border rounded" 
                    onChange={(e) => setFormData({...formData, passwordHash: e.target.value})} required />
                <button type="submit" className="w-full bg-bmsPrimary text-white py-2 rounded">Register</button>
            </form>
        </div>
    );
};

export default RegisterPage;