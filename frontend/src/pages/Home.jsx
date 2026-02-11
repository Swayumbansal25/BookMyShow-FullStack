import React, { useEffect, useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { movieService } from '../api/movieService';

const Home = () => {
    const [movies, setMovies] = useState([]);
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();
    const location = useLocation();

    const queryParams = new URLSearchParams(location.search);
    const searchTerm = queryParams.get('search')?.toLowerCase() || "";

    useEffect(() => {
        const fetchMovies = async () => {
            setLoading(true);
            try {
                const movieData = await movieService.getAllMovies();
                setMovies(movieData);
            } catch (error) {
                console.error("Failed to fetch:", error);
            } finally {
                setLoading(false);
            }
        };
        fetchMovies();
    }, []);

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
                </div>
                
                {filteredMovies.length > 0 ? (
                    <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-5 gap-8">
                        {filteredMovies.map((movie) => (
                            <div 
                                key={movie.movieId} 
                                onClick={() => navigate(`/movie/${movie.movieId}`)}
                                className="bg-white rounded-2xl shadow-sm hover:shadow-2xl transition-all duration-500 cursor-pointer overflow-hidden group border border-gray-100 relative"
                            >
                                {/* Rating Badge */}
                                <div className="absolute top-4 left-4 bg-black/70 backdrop-blur-md text-white px-2 py-1 rounded-md flex items-center gap-1 z-10">
                                    <span className="text-yellow-400 text-xs">⭐</span>
                                    <span className="text-[10px] font-black uppercase">
                                        {(4 + (movie.movieId % 10) / 10).toFixed(1)}
                                    </span>
                                </div>

                                {/* Movie Image: Replaces the Emoji */}
                                <div className="h-80 bg-[#1A1C24] overflow-hidden">
                                    {movie.movieImage ? (
                                        <img 
                                            src={movie.movieImage} 
                                            alt={movie.movieName}
                                            className="w-full h-full object-cover group-hover:scale-110 transition-transform duration-500"
                                            onError={(e) => { e.target.src = 'https://via.placeholder.com/300x450?text=No+Image'; }}
                                        />
                                    ) : (
                                        <div className="flex items-center justify-center h-full text-4xl">🎬</div>
                                    )}
                                </div>

                                <div className="p-5">
                                    <h3 className="font-black text-gray-900 truncate uppercase text-sm mb-1">{movie.movieName}</h3>
                                    <p className="text-xs font-bold text-gray-400 mb-4">{movie.genre}</p>
                                    
                                    <div className="flex justify-between items-center text-[9px] font-black text-gray-400 mb-1 uppercase tracking-widest">
                                        <span>Interest</span>
                                        <span className="text-[#F84464]">{(70 + (movie.movieId % 30))}%</span>
                                    </div>
                                    <div className="w-full bg-gray-100 h-1 rounded-full">
                                        <div 
                                            className="bg-[#F84464] h-1 rounded-full" 
                                            style={{ width: `${70 + (movie.movieId % 30)}%` }}
                                        ></div>
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                ) : (
                    <div className="text-center py-24 bg-white rounded-[3rem] border-2 border-dashed border-gray-100">
                        <p className="text-xl text-gray-800 font-black uppercase tracking-widest">No matches found</p>
                    </div>
                )}
            </main>
        </div>
    );
};

export default Home;