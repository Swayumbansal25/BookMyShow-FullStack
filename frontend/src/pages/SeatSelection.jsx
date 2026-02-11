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
                const response = await axiosInstance.get(`/ShowSeats/show/${showId}?t=${Date.now()}`);
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

    // Grouping seats into rows of 10 for professional labeling
    const seatsPerRow = 10;
    const rows = [];
    for (let i = 0; i < seats.length; i += seatsPerRow) {
        rows.push(seats.slice(i, i + seatsPerRow));
    }

    const toggleSeat = (seat) => {
        const isSelected = selectedSeats.some(s => s.showSeatId === seat.showSeatId);
        if (isSelected) {
            setSelectedSeats(selectedSeats.filter(s => s.showSeatId !== seat.showSeatId));
        } else {
            setSelectedSeats([...selectedSeats, seat]);
        }
    };

    if (loading) return (
        <div className="flex flex-col justify-center items-center h-screen bg-gray-50">
            <div className="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-[#F84464]"></div>
            <p className="mt-4 font-bold text-gray-500 uppercase tracking-widest text-xs">Loading Seat Map</p>
        </div>
    );

    return (
        <div className="min-h-screen bg-gray-100 p-4 md:p-10">
            <div className="max-w-5xl mx-auto bg-white p-6 md:p-10 rounded-[3rem] shadow-xl">
                <div className="flex justify-between items-center mb-10">
                    <button onClick={() => navigate(-1)} className="text-gray-400 hover:text-black font-black text-[10px] uppercase tracking-widest">← Back</button>
                    <h2 className="text-xl font-black text-gray-800 uppercase tracking-tighter">Choose <span className="text-[#F84464]">Seats</span></h2>
                    <div className="w-10"></div>
                </div>

                <div className="relative mb-16 text-center">
                    <div className="w-full h-2 bg-gray-100 rounded-full mb-2"></div>
                    <p className="text-[9px] font-black text-gray-300 uppercase tracking-[0.5em]">Screen This Way</p>
                </div>

                {/* Grid with Row Labels */}
                <div className="flex flex-col gap-4 mb-12 overflow-x-auto pb-4">
                    {rows.map((rowSeats, rowIndex) => {
                        const rowLabel = String.fromCharCode(65 + rowIndex); // A, B, C...
                        return (
                            <div key={rowLabel} className="flex items-center gap-4 min-w-max justify-center">
                                <span className="w-6 text-[11px] font-black text-gray-300">{rowLabel}</span>
                                <div className="flex gap-3">
                                    {rowSeats.map((seat, colIndex) => {
                                        const seatLabel = `${rowLabel}${colIndex + 1}`; // A1, A2...
                                        const isSold = seat.status === "Booked";
                                        const isSelected = selectedSeats.some(s => s.showSeatId === seat.showSeatId);

                                        return (
                                            <button
                                                key={seat.showSeatId}
                                                disabled={isSold}
                                                onClick={() => toggleSeat({ ...seat, seatLabel })}
                                                className={`w-9 h-9 rounded-lg text-[10px] font-black transition-all duration-300 border-2
                                                    ${isSold ? 'bg-gray-100 border-transparent text-gray-300 cursor-not-allowed' 
                                                    : isSelected ? 'bg-[#F84464] border-[#F84464] text-white shadow-lg shadow-[#f8446444] -translate-y-1' 
                                                    : 'bg-white border-green-500/20 text-green-600 hover:border-green-500 hover:bg-green-50'}`}
                                            >
                                                {colIndex + 1}
                                            </button>
                                        );
                                    })}
                                </div>
                            </div>
                        );
                    })}
                </div>

                {/* Status Legend */}
                <div className="flex justify-center gap-10 py-6 border-t border-gray-50 text-[9px] font-black uppercase text-gray-400">
                    <div className="flex items-center gap-2"><div className="w-3 h-3 border-2 border-green-500/20 rounded-sm"></div> Available</div>
                    <div className="flex items-center gap-2"><div className="w-3 h-3 bg-[#F84464] rounded-sm"></div> Selected</div>
                    <div className="flex items-center gap-2"><div className="w-3 h-3 bg-gray-100 rounded-sm"></div> Sold</div>
                </div>

                {/* Floating Confirmation Bar */}
                {selectedSeats.length > 0 && (
                    <div className="mt-10 bg-gray-900 text-white p-6 rounded-[2rem] flex flex-col md:flex-row justify-between items-center animate-in fade-in slide-in-from-bottom-4 duration-500">
                        <div className="mb-4 md:mb-0 text-center md:text-left">
                            <p className="text-[10px] text-gray-500 font-black uppercase tracking-widest mb-1">Seats Selected</p>
                            <div className="flex flex-wrap gap-2 justify-center md:justify-start">
                                {selectedSeats.map(s => (
                                    <span key={s.showSeatId} className="bg-white/10 px-3 py-1 rounded-md text-xs font-black text-[#F84464]">{s.seatLabel}</span>
                                ))}
                            </div>
                        </div>
                        <button 
                            onClick={() => navigate(`/payment/${showId}`, { state: { selectedSeats } })}
                            className="bg-[#F84464] px-12 py-4 rounded-xl font-black text-xs uppercase tracking-widest hover:bg-[#d63a56] transition-all active:scale-95 shadow-xl shadow-[#f8446444]"
                        >
                            Proceed to Pay ₹{selectedSeats.length * 250}
                        </button>
                    </div>
                )}
            </div>
        </div>
    );
};

export default SeatSelection;