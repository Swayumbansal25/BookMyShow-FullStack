import React, { useState } from 'react';
import { useLocation, useParams, useNavigate } from 'react-router-dom';
import axiosInstance from '../api/axiosInstance';

const BookingSummary = () => {
    const { showId } = useParams();
    const { state } = useLocation();
    const navigate = useNavigate();
    const [isProcessing, setIsProcessing] = useState(false);
    const selectedSeats = state?.selectedSeats || [];

    const handleConfirmBooking = async () => {
        if (isProcessing) return;
        setIsProcessing(true);

        try {
            // 1. Get User ID from Local Storage
            const user = JSON.parse(localStorage.getItem('user'));
            if (!user || !user.userId) {
                alert("User session not found. Please log in again.");
                return navigate('/login');
            }

            // 2. Create the Booking - Aligned with your CreateBookingDto
            const bookingPayload = {
                userId: user.userId,
                showId: parseInt(showId),
                showSeatIds: selectedSeats 
            };

            console.log("Sending Booking Payload:", bookingPayload);
            const bookingResponse = await axiosInstance.post('/Bookings', bookingPayload);

            // 3. Process the Payment using the ID returned from the booking
            const bookingId = bookingResponse.data.id || bookingResponse.data.bookingId;
            
            await axiosInstance.post('/Payments', {
                bookingId: bookingId,
                amount: selectedSeats.length * 1000, 
                paymentMethod: "Online",
                transactionId: "TXN-" + Date.now()
            });

            // 4. Success!
            navigate('/success');
        } catch (error) {
            console.error("Booking Error Details:", error.response?.data || error.message);
            // Specifically catching your "Seat not available" error
            alert(error.response?.data || "Booking failed. The seats might have just been taken.");
        } finally {
            setIsProcessing(false);
        }
    };

    return (
        <div className="min-h-screen bg-gray-50 flex items-center justify-center p-6">
            <div className="max-w-md w-full bg-white rounded-3xl shadow-xl overflow-hidden border border-gray-100">
                <div className="bg-[#2E3147] p-8 text-white text-center">
                    <h2 className="text-xl font-bold uppercase tracking-widest">Final Summary</h2>
                </div>
                
                <div className="p-8 space-y-6">
                    <div className="flex justify-between border-b pb-4">
                        <span className="text-gray-500 font-medium">Seats Selected</span>
                        <span className="font-bold text-[#F84464]">{selectedSeats.length}</span>
                    </div>

                    <div className="space-y-2">
                        <p className="text-xs font-bold text-gray-400 uppercase">Total Amount</p>
                        <p className="text-3xl font-extrabold text-gray-900">₹{selectedSeats.length * 1000}</p>
                    </div>

                    <button 
                        onClick={handleConfirmBooking}
                        disabled={isProcessing}
                        className={`w-full text-white py-4 rounded-xl font-bold text-lg transition-all shadow-lg active:scale-95 ${
                            isProcessing ? 'bg-gray-400 cursor-not-allowed' : 'bg-[#F84464] hover:bg-[#d63a56]'
                        }`}
                    >
                        {isProcessing ? "Processing..." : "Confirm & Pay"}
                    </button>
                    
                    <button 
                        onClick={() => navigate(-1)} 
                        disabled={isProcessing}
                        className="w-full text-gray-400 text-sm font-semibold hover:text-gray-600"
                    >
                        Go Back
                    </button>
                </div>
            </div>
        </div>
    );
};

export default BookingSummary;