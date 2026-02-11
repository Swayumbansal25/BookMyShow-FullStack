import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { userService } from '../api/userService';

const LoginPage = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();
        try {
            console.log("Attempting login for:", email);
            const response = await userService.login(email, password);
            
            // Log the full response to see the structure (e.g., is it response.data or just response?)
            console.log("Backend Login Response:", response);

            if (response) {
                // Force a check: does the response have a userId?
                // If your backend returns the user inside a 'value' or 'data' property, 
                // we store the whole object.
                const userToStore = response.data ? response.data : response;
                
                localStorage.setItem('user', JSON.stringify(userToStore));
                
                // Verify immediately in console
                console.log("Stored in LocalStorage:", localStorage.getItem('user'));
                
                navigate('/home');
            } else {
                alert("Login failed: No response data received.");
            }
        } catch (error) {
            console.error("Login Error Details:", error.response || error);
            alert("Login failed. Check console for error details.");
        }
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-gray-50 px-4">
            <div className="max-w-md w-full bg-white rounded-2xl shadow-xl p-10 border border-gray-100">
                <div className="text-center mb-10">
                    <h2 className="text-4xl font-extrabold text-[#F84464] tracking-tighter">
                        book<span className="bg-[#F84464] text-white px-1 ml-1 rounded">my</span>show
                    </h2>
                    <p className="text-gray-500 mt-3 font-medium">Please sign in to continue</p>
                </div>
                
                <form onSubmit={handleLogin} className="space-y-5">
                    <div>
                        <label className="block text-sm font-bold text-gray-700 mb-1 ml-1">Email Address</label>
                        <input
                            type="email"
                            required
                            className="w-full px-4 py-3 border border-gray-300 rounded-xl focus:ring-2 focus:ring-[#F84464] outline-none"
                            onChange={(e) => setEmail(e.target.value)}
                        />
                    </div>

                    <div>
                        <label className="block text-sm font-bold text-gray-700 mb-1 ml-1">Password</label>
                        <input
                            type="password"
                            required
                            className="w-full px-4 py-3 border border-gray-300 rounded-xl focus:ring-2 focus:ring-[#F84464] outline-none"
                            onChange={(e) => setPassword(e.target.value)}
                        />
                    </div>

                    <button
                        type="submit"
                        className="w-full bg-[#F84464] text-white py-3 rounded-xl font-bold text-lg hover:bg-[#d63a56] shadow-lg transition-all"
                    >
                        Login
                    </button>
                </form>

                <div className="mt-8 text-center text-sm">
                    <p className="text-gray-600">
                        Don't have an account?{' '}
                        <Link to="/register" className="font-bold text-[#F84464] hover:underline">
                            Register
                        </Link>
                    </p>
                </div>
            </div>
        </div>
    );
};

export default LoginPage;