import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axiosInstance from '../api/axiosInstance';

const SeatSelection = () => {
    const { showId } = useParams();
    const navigate = useNavigate();
    const [seats, setSeats] = useState([]);
    const [loading, setLoading] = useState(true);
    const [selectedSeats, setSelectedSeats] = useState([]);

    useEffect(() => {
        const fetchSeats = async () => {
            try {
                // Fetch the seat status with a timestamp to avoid stale data
                const response = await axiosInstance.get(`/ShowSeats/show/${showId}?t=${Date.now()}`);
                console.log("ShowSeats API Response:", response.data);
                
                // CRITICAL FIX: Sort by seatNumber to ensure the grid starts from 1, 2, 3...
                const sortedSeats = (response.data || []).sort((a, b) => a.seatNumber - b.seatNumber);
                setSeats(sortedSeats);
            } catch (error) {
                console.error("Error fetching seats:", error);
            } finally {
                setLoading(false);
            }
        };
        fetchSeats();
    }, [showId]);

    const toggleSeat = (seatId) => {
        if (selectedSeats.includes(seatId)) {
            setSelectedSeats(selectedSeats.filter(id => id !== seatId));
        } else {
            setSelectedSeats([...selectedSeats, seatId]);
        }
    };

    if (loading) return (
        <div className="flex flex-col justify-center items-center h-screen bg-gray-50">
            <div className="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-[#F84464]"></div>
            <p className="mt-4 font-bold text-gray-500 uppercase tracking-widest text-xs">Loading Seat Map</p>
        </div>
    );

    return (
        <div className="min-h-screen bg-gray-100 p-6 md:p-10">
            <div className="max-w-4xl mx-auto bg-white p-8 rounded-3xl shadow-xl">
                <div className="flex justify-between items-center mb-12">
                    <button 
                        onClick={() => navigate(-1)} 
                        className="text-gray-400 hover:text-black font-bold text-sm transition-colors"
                    >
                        ← BACK
                    </button>
                    <h2 className="text-xl font-black text-gray-800 uppercase tracking-tighter">
                        SELECT <span className="text-[#F84464]">YOUR SEATS</span>
                    </h2>
                    <div className="w-10"></div>
                </div>
                
                {/* Screen Indicator */}
                <div className="relative mb-20 text-center">
                    <div className="w-full h-1 bg-gradient-to-r from-transparent via-gray-300 to-transparent"></div>
                    <p className="text-[10px] font-bold text-gray-400 uppercase mt-2 tracking-[0.4em]">All eyes this way</p>
                </div>

                {/* Seat Grid */}
                <div className="grid grid-cols-5 sm:grid-cols-8 md:grid-cols-10 gap-4 justify-center mb-16">
                    {seats.map(seat => {
                        const isSold = seat.status === "Booked"; 
                        const isSelected = selectedSeats.includes(seat.showSeatId);

                        return (
                            <button
                                key={seat.showSeatId}
                                disabled={isSold}
                                onClick={() => toggleSeat(seat.showSeatId)}
                                className={`w-10 h-10 rounded-lg text-[11px] font-bold transition-all duration-200 flex items-center justify-center
                                    ${isSold 
                                        ? 'bg-gray-200 cursor-not-allowed text-gray-400 border-none opacity-60' 
                                        : isSelected 
                                            ? 'bg-[#F84464] text-white shadow-lg scale-110 z-10' 
                                            : 'bg-white border-2 border-green-500 text-green-600 hover:bg-green-500 hover:text-white'}`}
                            >
                                {/* UPDATED: Always use seatNumber so every Audi starts from 1 */}
                                {seat.seatNumber}
                            </button>
                        );
                    })}
                </div>

                {/* Legend */}
                <div className="flex flex-wrap justify-center gap-8 py-8 border-t border-gray-100 text-[10px] font-black uppercase tracking-widest text-gray-400">
                    <div className="flex items-center gap-3">
                        <div className="w-4 h-4 border-2 border-green-500 rounded-md"></div> Available
                    </div>
                    <div className="flex items-center gap-3">
                        <div className="w-4 h-4 bg-[#F84464] rounded-md shadow-sm"></div> Selected
                    </div>
                    <div className="flex items-center gap-3">
                        <div className="w-4 h-4 bg-gray-200 rounded-md"></div> Sold
                    </div>
                </div>

                {/* Final Booking Action Bar */}
                {selectedSeats.length > 0 && (
                    <div className="mt-8 bg-gray-50 -mx-8 -mb-8 p-8 flex flex-col md:flex-row justify-between items-center rounded-b-3xl border-t border-gray-200">
                        <div className="mb-4 md:mb-0 text-center md:text-left">
                            <p className="text-[10px] text-gray-400 font-black uppercase tracking-widest">Total Selection</p>
                            <p className="text-2xl font-black text-gray-800">
                                {selectedSeats.length} {selectedSeats.length === 1 ? 'Seat' : 'Seats'}
                            </p>
                        </div>
                        <button 
                            onClick={() => navigate(`/payment/${showId}`, { state: { selectedSeats } })}
                            className="w-full md:w-auto bg-[#F84464] text-white px-16 py-4 rounded-2xl font-black text-sm uppercase tracking-widest hover:bg-[#d63a56] transition-all shadow-xl hover:shadow-[#f8446444] active:scale-95"
                        >
                            Confirm Selection
                        </button>
                    </div>
                )}
            </div>
        </div>
    );
};

export default SeatSelection;