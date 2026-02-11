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
                // 1. Get CityId by name
                const cityRes = await axiosInstance.get('/Cities/paged');
                const cities = cityRes.data.data || cityRes.data;
                const targetCity = cities.find(c => c.cityName.toLowerCase() === selectedCityName.toLowerCase());

                if (!targetCity) return;

                // 2. Fetch theatres for that CityId
                const theatreRes = await axiosInstance.get('/Theatres/paged');
                const allTheatres = theatreRes.data.data || theatreRes.data;
                const filtered = allTheatres.filter(t => t.cityId === targetCity.cityId);

                // 3. Fetch specific shows
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
                <h2 className="text-xl font-black text-gray-800 uppercase">City Required</h2>
                <p className="text-gray-500 mt-2">Please select a city in the Navbar to view showtimes.</p>
            </div>
        );
    }

    if (loading) return <div className="p-20 text-center animate-pulse">Loading showtimes...</div>;

    return (
        <div className="min-h-screen bg-gray-50 p-6 md:p-10">
            <h2 className="text-2xl font-black mb-8 uppercase">Theatres in <span className="text-[#F84464]">{selectedCityName}</span></h2>
            {theatres.length > 0 ? (
                theatres.map(t => (
                    <div key={t.theatreId} className="bg-white p-8 rounded-3xl shadow-sm border mb-6">
                        <h3 className="font-black text-lg uppercase mb-6">{t.theatreName}</h3>
                        <div className="flex flex-wrap gap-4">
                            {t.shows.map(s => (
                                <button key={s.showId} onClick={() => navigate(`/seats/${s.showId}`)}
                                    className="border border-[#F84464] text-[#F84464] px-6 py-3 rounded-xl font-bold hover:bg-[#F84464] hover:text-white transition-all">
                                    {s.startTime.substring(0, 5)}
                                </button>
                            ))}
                        </div>
                    </div>
                ))
            ) : (
                <div className="p-20 text-center bg-white rounded-3xl border-2 border-dashed">No shows found.</div>
            )}
        </div>
    );
};

export default ShowtimeSelection;