import React, { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import axiosInstance from '../api/axiosInstance';

const Navbar = () => {
    const navigate = useNavigate();
    const [states, setStates] = useState([]);
    const [cities, setCities] = useState([]);
    const [selectedState, setSelectedState] = useState("");
    const [selectedCity, setSelectedCity] = useState(localStorage.getItem('selectedCity') || "");
    const [searchTerm, setSearchTerm] = useState("");
    
    const user = JSON.parse(localStorage.getItem('user'));

    useEffect(() => {
        const fetchStates = async () => {
            try {
                const res = await axiosInstance.get('/States');
                setStates(res.data.data || res.data); 
            } catch (err) { console.error("Error fetching states:", err); }
        };
        fetchStates();
    }, []);

    const handleStateChange = async (stateId) => {
        setSelectedState(stateId);
        setSelectedCity("");
        try {
            const res = await axiosInstance.get(`/Cities/paged?StateId=${stateId}`);
            setCities(res.data.data || res.data);
        } catch (err) { console.error("Error fetching cities:", err); }
    };

    const handleCityChange = (cityName) => {
        setSelectedCity(cityName);
        localStorage.setItem('selectedCity', cityName); 
        navigate(`/home?city=${cityName}`);
    };

    const handleLogout = () => {
        localStorage.clear(); // Clears everything for a fresh login
        navigate('/login');
    };

    return (
        <nav className="bg-[#2E3147] text-white px-8 py-3 flex flex-col md:flex-row justify-between items-center shadow-lg gap-4 relative z-50">
            <Link to="/home" className="text-xl font-black tracking-tighter uppercase shrink-0">
                BOOK<span className="text-[#F84464]">MY</span>SHOW
            </Link>

            <div className="flex flex-grow max-w-3xl w-full gap-2">
                <select value={selectedState} onChange={(e) => handleStateChange(e.target.value)}
                    className="bg-[#1F2133] text-[10px] font-bold uppercase py-2 px-3 rounded-md outline-none cursor-pointer">
                    <option value="">Select State</option>
                    {states.map(s => <option key={s.stateId} value={s.stateId}>{s.stateName}</option>)}
                </select>

                <select disabled={!selectedState} value={selectedCity} onChange={(e) => handleCityChange(e.target.value)}
                    className="bg-[#1F2133] text-[10px] font-bold uppercase py-2 px-3 rounded-md outline-none cursor-pointer disabled:opacity-30">
                    <option value="">{selectedState ? "Select City" : "Pick State First"}</option>
                    {cities.map(c => <option key={c.cityId} value={c.cityName}>{c.cityName}</option>)}
                </select>

                <form className="flex-grow relative" onSubmit={(e) => e.preventDefault()}>
                    <input type="text" placeholder="Search for Movies..." value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)}
                        className="w-full bg-[#1F2133] text-sm border-none rounded-md py-2 px-10 outline-none" />
                    <span className="absolute left-3 top-2">🔍</span>
                </form>
            </div>

            <div className="flex items-center gap-6 text-[11px] font-bold uppercase shrink-0">
                <Link to="/home">Movies</Link>
                <Link to="/my-bookings">My Bookings</Link>
                {user && (
                    <div className="flex items-center gap-4 border-l border-gray-600 pl-4">
                        <div className="flex flex-col items-end leading-none">
                            <span className="text-[#F84464] text-[9px] mb-1">{selectedCity || "No City Selected"}</span>
                            <span className="text-gray-400 text-[8px] lowercase">{user.email}</span>
                        </div>
                        <button onClick={handleLogout} className="bg-[#F84464] hover:bg-[#d63a56] text-white px-3 py-1.5 rounded text-[9px] font-black uppercase">
                            Sign Out
                        </button>
                    </div>
                )}
            </div>
        </nav>
    );
};

export default Navbar;