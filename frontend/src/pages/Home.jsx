import React, { useEffect, useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { movieService } from '../api/movieService';

const Home = () => {
    const [movies, setMovies] = useState([]);
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();
    const location = useLocation();

    // Step 1: Extract search term from the URL
    const queryParams = new URLSearchParams(location.search);
    const searchTerm = queryParams.get('search')?.toLowerCase() || "";

    useEffect(() => {
        const fetchMovies = async () => {
            setLoading(true);
            try {
                const movieData = await movieService.getAllMovies();
                console.log("Setting Movies State with:", movieData);
                setMovies(movieData);
            } catch (error) {
                console.error("Failed to fetch:", error);
            } finally {
                setLoading(false);
            }
        };
        fetchMovies();
    }, []);

    // Step 2: Filter movies based on the search term
    const filteredMovies = movies.filter(movie => 
        movie.movieName.toLowerCase().includes(searchTerm) || 
        movie.genre?.toLowerCase().includes(searchTerm)
    );

    if (loading) return (
        <div className="h-screen flex flex-col items-center justify-center bg-gray-50">
            <div className="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-[#F84464] mb-4"></div>
            <p className="text-xl font-bold text-gray-700">Connecting to BookMyShow API...</p>
        </div>
    );

    return (
        <div className="min-h-screen bg-gray-50 font-sans">
            <main className="max-w-7xl mx-auto py-10 px-6">
                <div className="flex items-center justify-between mb-10">
                    <h2 className="text-2xl font-black text-gray-800 border-l-8 border-[#F84464] pl-4 uppercase tracking-tighter">
                        {searchTerm ? `Results for "${searchTerm}"` : "Recommended Movies"}
                    </h2>
                    {searchTerm && (
                        <button 
                            onClick={() => navigate('/home')}
                            className="text-xs font-black text-[#F84464] hover:underline uppercase tracking-widest"
                        >
                            Clear Search
                        </button>
                    )}
                </div>
                
                {filteredMovies.length > 0 ? (
                    <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-5 gap-8">
                        {filteredMovies.map((movie) => (
                            <div 
                                key={movie.movieId} 
                                onClick={() => navigate(`/movie/${movie.movieId}`)}
                                className="bg-white rounded-2xl shadow-sm hover:shadow-2xl transition-all duration-500 cursor-pointer overflow-hidden group border border-gray-100 relative"
                            >
                                {/* Rating Badge Logic */}
                                <div className="absolute top-4 left-4 bg-black/70 backdrop-blur-md text-white px-2 py-1 rounded-md flex items-center gap-1 z-10 scale-90 group-hover:scale-100 transition-transform">
                                    <span className="text-yellow-400 text-xs">⭐</span>
                                    <span className="text-[10px] font-black uppercase">
                                        {(4 + (movie.movieId % 10) / 10).toFixed(1)}/5
                                    </span>
                                </div>

                                {/* Placeholder Image with Title Overlay */}
                                <div className="h-80 bg-[#1A1C24] flex flex-col items-center justify-center text-gray-500 group-hover:bg-[#12141a] transition-colors relative">
                                    <span className="text-5xl mb-4 group-hover:scale-110 transition-transform">🎬</span>
                                    <span className="font-black uppercase tracking-[0.2em] text-[10px] px-6 text-center leading-relaxed">
                                        {movie.movieName}
                                    </span>
                                </div>

                                {/* Movie Info Body */}
                                <div className="p-5">
                                    <h3 className="font-black text-gray-900 truncate uppercase text-sm mb-1">{movie.movieName}</h3>
                                    <p className="text-xs font-bold text-gray-400 mb-4">{movie.genre}</p>
                                    
                                    {/* Hype/Rating Progress Bar */}
                                    <div className="flex justify-between items-center text-[9px] font-black text-gray-400 mb-1 uppercase tracking-widest">
                                        <span>Interest</span>
                                        <span className="text-[#F84464]">{(70 + (movie.movieId % 30))}%</span>
                                    </div>
                                    <div className="w-full bg-gray-100 h-1 rounded-full mb-5">
                                        <div 
                                            className="bg-[#F84464] h-1 rounded-full shadow-[0_0_10px_rgba(248,68,100,0.5)]" 
                                            style={{ width: `${70 + (movie.movieId % 30)}%` }}
                                        ></div>
                                    </div>

                                    <div className="flex justify-between items-center pt-4 border-t border-gray-50">
                                        <span className="text-[9px] font-black text-gray-400 uppercase tracking-widest">
                                            {movie.language} • {movie.durationMinutes}m
                                        </span>
                                        <span className="text-[9px] font-black text-gray-300 uppercase">
                                            {(10 + movie.movieId * 2)}K Votes
                                        </span>
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                ) : (
                    <div className="text-center py-24 bg-white rounded-[3rem] shadow-sm border-2 border-dashed border-gray-100">
                        <div className="text-5xl mb-6 opacity-20 grayscale">🔎</div>
                        <p className="text-xl text-gray-800 font-black uppercase tracking-widest mb-2">
                            No matches found
                        </p>
                        <p className="text-gray-400 text-sm font-medium mb-8">
                            We couldn't find any movies matching "{searchTerm}"
                        </p>
                        <button 
                            onClick={() => navigate('/home')}
                            className="bg-[#F84464] text-white px-10 py-3 rounded-xl font-black text-xs uppercase tracking-widest hover:shadow-xl transition-all"
                        >
                            View All Movies
                        </button>
                    </div>
                )}
            </main>
        </div>
    );
};

export default Home;