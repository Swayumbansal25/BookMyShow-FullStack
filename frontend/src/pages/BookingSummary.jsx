import React, { useState } from 'react';
import { useLocation, useParams, useNavigate } from 'react-router-dom';
import axiosInstance from '../api/axiosInstance';

const BookingSummary = () => {
    const { showId } = useParams();
    const { state } = useLocation();
    const navigate = useNavigate();
    const [isProcessing, setIsProcessing] = useState(false);
    
    // Extracting selected seats from state passed by SeatSelection
    const selectedSeats = state?.selectedSeats || [];

    const handleConfirmBooking = async () => {
        if (isProcessing) return;
        setIsProcessing(true);

        try {
            const user = JSON.parse(localStorage.getItem('user'));
            if (!user || !user.userId) {
                alert("User session not found. Please log in again.");
                return navigate('/login');
            }

            // Extracting ONLY the IDs for the backend payload
            const bookingPayload = {
                userId: user.userId,
                showId: parseInt(showId),
                showSeatIds: selectedSeats.map(s => s.showSeatId) 
            };

            const bookingResponse = await axiosInstance.post('/Bookings', bookingPayload);
            const bookingId = bookingResponse.data.id || bookingResponse.data.bookingId;
            
            await axiosInstance.post('/Payments', {
                bookingId: bookingId,
                amount: selectedSeats.length * 250, // Updated price to match Selection Bar
                paymentMethod: "Online",
                transactionId: "TXN-" + Date.now()
            });

            navigate('/success');
        } catch (error) {
            console.error("Booking Error:", error.response?.data || error.message);
            alert(error.response?.data || "Booking failed.");
        } finally {
            setIsProcessing(false);
        }
    };

    return (
        <div className="min-h-screen bg-gray-100 flex items-center justify-center p-6">
            <div className="max-w-md w-full bg-white rounded-[3rem] shadow-2xl overflow-hidden border border-gray-100">
                <div className="bg-[#1A1C24] p-10 text-white text-center">
                    <p className="text-[10px] font-black uppercase tracking-[0.4em] mb-2 text-gray-500">Checkout</p>
                    <h2 className="text-2xl font-black uppercase tracking-tighter">Booking <span className="text-[#F84464]">Summary</span></h2>
                </div>
                
                <div className="p-10 space-y-8">
                    {/* Professional Seat Display */}
                    <div className="space-y-3">
                        <p className="text-[10px] font-black text-gray-400 uppercase tracking-widest">Selected Seats</p>
                        <div className="flex flex-wrap gap-2">
                            {selectedSeats.map(seat => (
                                <span key={seat.showSeatId} className="bg-gray-50 border border-gray-200 px-4 py-2 rounded-xl text-sm font-black text-[#F84464]">
                                    {seat.seatLabel}
                                </span>
                            ))}
                        </div>
                    </div>

                    <div className="pt-6 border-t border-gray-50 space-y-2">
                        <div className="flex justify-between items-center">
                            <span className="text-xs font-bold text-gray-400 uppercase">Ticket Price ({selectedSeats.length})</span>
                            <span className="text-sm font-black text-gray-800">₹{selectedSeats.length * 250}</span>
                        </div>
                        <div className="flex justify-between items-center">
                            <span className="text-xs font-bold text-gray-400 uppercase">Convenience Fee</span>
                            <span className="text-sm font-black text-gray-800">₹0.00</span>
                        </div>
                        <div className="flex justify-between items-center pt-4">
                            <span className="text-xs font-black text-gray-900 uppercase tracking-widest">Amount Payable</span>
                            <span className="text-3xl font-black text-[#F84464]">₹{selectedSeats.length * 250}</span>
                        </div>
                    </div>

                    <div className="space-y-4 pt-4">
                        <button 
                            onClick={handleConfirmBooking}
                            disabled={isProcessing}
                            className={`w-full py-5 rounded-2xl font-black text-xs uppercase tracking-[0.2em] transition-all shadow-xl active:scale-95 ${
                                isProcessing ? 'bg-gray-200 text-gray-400 cursor-not-allowed' : 'bg-[#F84464] text-white hover:bg-[#d63a56] shadow-[#f8446444]'
                            }`}
                        >
                            {isProcessing ? "Processing..." : "Secure Payment"}
                        </button>
                        
                        <button 
                            onClick={() => navigate(-1)} 
                            disabled={isProcessing}
                            className="w-full text-gray-300 text-[10px] font-black uppercase tracking-widest hover:text-gray-500 transition-colors"
                        >
                            Cancel & Go Back
                        </button>
                    </div>
                </div>
                
                <div className="bg-gray-50 p-6 text-center">
                    <p className="text-[9px] font-bold text-gray-400 uppercase tracking-widest">
                        By proceeding, you agree to our Terms & Conditions
                    </p>
                </div>
            </div>
        </div>
    );
};

export default BookingSummary;