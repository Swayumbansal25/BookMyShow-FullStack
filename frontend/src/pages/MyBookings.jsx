import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axiosInstance from '../api/axiosInstance';

const MyBookings = () => {
    const [bookings, setBookings] = useState([]);
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchMyBookings = async () => {
            try {
                // Retrieve user from Local Storage
                const user = JSON.parse(localStorage.getItem('user'));
                if (user?.userId) {
                    // Fetching from your specific User Bookings endpoint
                    const response = await axiosInstance.get(`/Bookings/user/${user.userId}`);
                    setBookings(response.data || []);
                }
            } catch (error) {
                console.error("Error fetching bookings:", error);
            } finally {
                setLoading(false);
            }
        };
        fetchMyBookings();
    }, []);

    if (loading) return (
        <div className="flex flex-col justify-center items-center h-screen bg-gray-50">
            <div className="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-[#F84464]"></div>
            <p className="mt-4 font-bold text-gray-500 uppercase tracking-widest text-xs">Loading Tickets...</p>
        </div>
    );

    return (
        <div className="min-h-screen bg-gray-50 p-6 md:p-10">
            <div className="max-w-4xl mx-auto">
                <div className="flex items-center justify-between mb-10">
                    <h2 className="text-3xl font-black text-gray-800 border-l-8 border-[#F84464] pl-4 uppercase tracking-tighter">
                        My <span className="text-[#F84464]">Bookings</span>
                    </h2>
                    <button 
                        onClick={() => navigate('/home')}
                        className="text-sm font-bold text-gray-400 hover:text-[#F84464] transition-colors"
                    >
                        + NEW BOOKING
                    </button>
                </div>

                {bookings.length > 0 ? (
                    <div className="grid gap-8">
                        {bookings.map((booking) => (
                            <div key={booking.bookingId} className="bg-white rounded-3xl shadow-sm hover:shadow-md transition-shadow border border-gray-100 overflow-hidden flex flex-col md:flex-row">
                                {/* Left Side: Branding/ID */}
                                <div className="bg-[#2E3147] p-8 text-white flex flex-col justify-center items-center min-w-[180px] text-center">
                                    <span className="text-[10px] font-black uppercase opacity-40 tracking-[0.2em] mb-1">Ticket ID</span>
                                    <span className="text-xl font-mono font-black">#BMS-{booking.bookingId}</span>
                                    <div className="mt-4 px-3 py-1 bg-[#F84464] rounded-full text-[9px] font-black uppercase tracking-widest">
                                        Confirmed
                                    </div>
                                </div>

                                {/* Right Side: Details */}
                                <div className="p-8 flex-grow flex flex-col justify-between">
                                    <div className="flex justify-between items-start mb-6">
                                        <div>
                                            <p className="text-[10px] font-black text-gray-400 uppercase tracking-widest mb-1">Show Information</p>
                                            <h3 className="text-xl font-black text-gray-800">Show ID: {booking.showId}</h3>
                                            <p className="text-gray-500 text-xs font-medium">Payment Status: <span className="text-green-600 uppercase">Paid</span></p>
                                        </div>
                                        <div className="text-right">
                                            <p className="text-[10px] font-black text-gray-400 uppercase tracking-widest mb-1">Total Paid</p>
                                            <p className="text-2xl font-black text-[#F84464]">₹{booking.totalAmount || "1000"}</p>
                                        </div>
                                    </div>

                                    {/* Seat List */}
                                    <div className="flex flex-wrap gap-2 pt-4 border-t border-gray-50">
                                        {booking.seats?.length > 0 ? booking.seats.map(seat => (
                                            <span key={seat.seatId} className="bg-gray-50 px-4 py-2 rounded-xl border border-gray-100 text-[10px] font-black text-gray-600 uppercase tracking-wider">
                                                Seat {seat.seatNumber}
                                            </span>
                                        )) : (
                                            <span className="text-xs text-gray-400 italic">Confirmed Seat(s)</span>
                                        )}
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                ) : (
                    /* Step 1: The New Empty State Layout */
                    <div className="flex flex-col items-center justify-center py-24 bg-white rounded-[3rem] shadow-sm border-2 border-dashed border-gray-100">
                        <div className="w-24 h-24 bg-gray-50 rounded-full flex items-center justify-center text-4xl mb-6 grayscale opacity-50">
                            🎟️
                        </div>
                        <h3 className="text-2xl font-black text-gray-800 mb-2">No Bookings Yet</h3>
                        <p className="text-gray-400 text-sm mb-10 text-center max-w-xs font-medium">
                            Your movie tickets will appear here once you've completed a booking!
                        </p>
                        <button 
                            onClick={() => navigate('/home')}
                            className="bg-[#F84464] text-white px-10 py-4 rounded-2xl font-black text-sm uppercase tracking-widest hover:bg-[#d63a56] transition-all shadow-xl hover:shadow-[#f8446444] active:scale-95"
                        >
                            Explore Movies
                        </button>
                    </div>
                )}
            </div>
        </div>
    );
};

export default MyBookings;