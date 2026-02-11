import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axiosInstance from '../api/axiosInstance';

const ShowtimeSelection = () => {
    const { movieId } = useParams();
    const navigate = useNavigate();
    const [theatres, setTheatres] = useState([]);
    const [loading, setLoading] = useState(true);
    const selectedCityName = localStorage.getItem('selectedCity');

    useEffect(() => {
        const fetchTheatresAndShows = async () => {
            if (!selectedCityName) { setLoading(false); return; }
            setLoading(true);
            try {
                const cityRes = await axiosInstance.get('/Cities/paged');
                const cities = cityRes.data.data || cityRes.data;
                const targetCity = cities.find(c => c.cityName.toLowerCase() === selectedCityName.toLowerCase());

                if (!targetCity) return;

                const theatreRes = await axiosInstance.get('/Theatres/paged');
                const allTheatres = theatreRes.data.data || theatreRes.data;
                const filtered = allTheatres.filter(t => t.cityId === targetCity.cityId);

                const withShows = await Promise.all(filtered.map(async (t) => {
                    const res = await axiosInstance.get(`/Shows/paged?MovieId=${movieId}&TheatreId=${t.theatreId}`);
                    return { ...t, shows: res.data.data || res.data };
                }));

                setTheatres(withShows.filter(t => t.shows.length > 0));
            } catch (error) { console.error(error); } finally { setLoading(false); }
        };

        fetchTheatresAndShows();
    }, [movieId, selectedCityName]);

    if (!selectedCityName) {
        return (
            <div className="min-h-screen flex flex-col items-center justify-center bg-gray-50">
                <h2 className="text-xl font-black text-gray-800 uppercase tracking-tighter">City Required</h2>
                <p className="text-gray-500 mt-2 font-medium">Please select a city in the Navbar to view showtimes.</p>
            </div>
        );
    }

    if (loading) return (
        <div className="flex justify-center items-center h-screen bg-gray-50">
            <div className="animate-spin rounded-full h-10 w-10 border-t-2 border-b-2 border-[#F84464]"></div>
        </div>
    );

    return (
        <div className="min-h-screen bg-gray-50 p-6 md:p-12">
            <div className="max-w-5xl mx-auto">
                <header className="mb-10">
                    <h2 className="text-2xl font-black text-gray-900 uppercase tracking-tight flex items-center gap-3">
                        Theatres in <span className="text-[#F84464]">{selectedCityName}</span>
                        <div className="h-1 w-10 bg-gray-200 rounded-full"></div>
                    </h2>
                </header>

                <div className="space-y-4">
                    {theatres.length > 0 ? (
                        theatres.map(t => (
                            <div 
                                key={t.theatreId} 
                                className="bg-white p-6 rounded-2xl border border-gray-100 flex flex-col md:flex-row md:items-center justify-between transition-all duration-300 hover:border-[#F84464]/30 hover:shadow-md group"
                            >
                                {/* Left Side: Theatre Info */}
                                <div className="mb-4 md:mb-0">
                                    <h3 className="font-black text-gray-800 uppercase text-sm tracking-wide group-hover:text-[#F84464] transition-colors">
                                        {t.theatreName}
                                    </h3>
                                    <div className="flex items-center gap-4 mt-2">
                                        <span className="text-[10px] font-bold text-green-500 uppercase flex items-center gap-1">
                                            <span className="w-1.5 h-1.5 bg-green-500 rounded-full animate-pulse"></span>
                                            M-Ticket Available
                                        </span>
                                        <span className="text-[10px] font-bold text-gray-300 uppercase">Food & Beverage</span>
                                    </div>
                                </div>

                                {/* Right Side: Showtimes (Horizontal Layout) */}
                                <div className="flex flex-wrap gap-3">
                                    {t.shows.map(s => (
                                        <button 
                                            key={s.showId} 
                                            onClick={() => navigate(`/seats/${s.showId}`)}
                                            className="min-w-[100px] bg-white border border-gray-200 py-2 px-4 rounded-lg text-[11px] font-black text-[#4abd5d] uppercase tracking-wider hover:border-[#4abd5d] hover:bg-green-50 transition-all active:scale-95"
                                        >
                                            {s.startTime.substring(0, 5)}
                                            <p className="text-[8px] text-gray-400 mt-0.5 font-bold">4K Dolby 7.1</p>
                                        </button>
                                    ))}
                                </div>
                            </div>
                        ))
                    ) : (
                        <div className="py-20 text-center bg-white rounded-[2.5rem] border-2 border-dashed border-gray-100">
                            <p className="text-gray-400 font-black uppercase tracking-widest">No shows available for this movie in {selectedCityName}</p>
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
};

export default ShowtimeSelection;