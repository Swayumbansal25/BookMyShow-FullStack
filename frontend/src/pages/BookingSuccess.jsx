import React from 'react';
import { useNavigate } from 'react-router-dom';

const BookingSuccess = () => {
    const navigate = useNavigate();

    return (
        <div className="min-h-screen bg-green-50 flex items-center justify-center p-6">
            <div className="max-w-md w-full bg-white rounded-3xl shadow-2xl p-10 text-center border-t-8 border-green-500">
                <div className="text-6xl mb-6">✅</div>
                <h2 className="text-3xl font-extrabold text-gray-800 mb-2">Booking Confirmed!</h2>
                <p className="text-gray-500 mb-8">Your tickets have been successfully booked. Enjoy your movie!</p>
                
                <div className="bg-gray-50 border-2 border-dashed border-gray-200 p-6 rounded-2xl mb-8">
                    <p className="text-xs font-bold text-gray-400 uppercase tracking-widest">Digital Ticket ID</p>
                    <p className="text-xl font-mono font-bold text-gray-700 mt-1">#BMS-{Math.floor(Math.random() * 900000 + 100000)}</p>
                </div>

                <button 
                    onClick={() => navigate('/home')}
                    className="w-full bg-[#2E3147] text-white py-4 rounded-xl font-bold hover:bg-black transition shadow-lg"
                >
                    Back to Home
                </button>
            </div>
        </div>
    );
};

export default BookingSuccess;