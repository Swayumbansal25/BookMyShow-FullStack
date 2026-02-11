import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axiosInstance from '../api/axiosInstance';

const MovieDetails = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [movie, setMovie] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchMovieDetails = async () => {
            try {
                const response = await axiosInstance.get(`/Movies/${id}`);
                setMovie(response.data);
            } catch (error) {
                console.error("Error fetching movie details:", error);
            } finally {
                setLoading(false);
            }
        };
        fetchMovieDetails();
    }, [id]);

    if (loading) return (
        <div className="flex justify-center items-center h-screen bg-gray-50">
            <div className="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-[#F84464]"></div>
        </div>
    );

    if (!movie) return (
        <div className="p-20 text-center">
            <h2 className="text-2xl font-bold text-gray-400">Movie not found.</h2>
            <button onClick={() => navigate('/home')} className="mt-4 text-[#F84464] font-bold">Back to Home</button>
        </div>
    );

    return (
        <div className="min-h-screen bg-white">
            {/* Movie Hero Section */}
            <div className="relative h-[65vh] bg-[#1A1C24] flex items-center justify-center overflow-hidden">
                {/* Background Decoration - Now using the movie poster as a blurred background */}
                <div className="absolute inset-0 opacity-20">
                    <img 
                        src={movie.movieImage} 
                        alt="" 
                        className="w-full h-full object-cover blur-2xl scale-110"
                    />
                </div>
                
                {/* Content Overlay */}
                <div className="relative z-10 max-w-6xl w-full px-10 flex flex-col md:flex-row gap-12 items-end">
                    {/* Poster Image Section */}
                    <div className="w-64 h-96 bg-[#252833] rounded-2xl shadow-2xl border-4 border-white/5 overflow-hidden mb-[-120px] hidden md:block shrink-0 group">
                        {movie.movieImage ? (
                            <img 
                                src={movie.movieImage} 
                                alt={movie.movieName}
                                className="w-full h-full object-cover group-hover:scale-105 transition-transform duration-500"
                            />
                        ) : (
                            <div className="flex flex-col items-center justify-center h-full text-gray-700">
                                <span className="text-5xl mb-4">🎞️</span>
                                <span className="text-[10px] font-black uppercase tracking-[0.2em] px-8 text-center leading-loose">
                                    {movie.movieName}
                                </span>
                            </div>
                        )}
                    </div>

                    <div className="flex-grow pb-12">
                        <div className="flex items-center gap-4 mb-6">
                            <span className="bg-[#F84464] text-white px-3 py-1 rounded-md text-[10px] font-black uppercase tracking-widest shadow-lg shadow-[#f8446444]">
                                Now Showing
                            </span>
                            <div className="flex items-center gap-2 bg-white/10 backdrop-blur-md px-3 py-1 rounded-md border border-white/10">
                                <span className="text-yellow-400 text-sm">⭐</span>
                                <span className="text-white text-sm font-bold">{(4 + (movie.movieId % 10) / 10).toFixed(1)}/5</span>
                                <span className="text-gray-400 text-[10px] font-bold uppercase ml-1">
                                    {(10 + movie.movieId * 2)}K Votes
                                </span>
                            </div>
                        </div>

                        <h1 className="text-6xl font-black text-white uppercase tracking-tighter mb-6 leading-none">
                            {movie.movieName}
                        </h1>

                        <div className="flex flex-wrap gap-4 text-xs font-black text-gray-300 uppercase tracking-widest items-center">
                            <span className="bg-white/5 px-3 py-1 rounded">{movie.genre}</span>
                            <span className="w-1 h-1 bg-gray-600 rounded-full"></span>
                            <span>{movie.language}</span>
                            <span className="w-1 h-1 bg-gray-600 rounded-full"></span>
                            <span>{movie.durationMinutes} Minutes</span>
                        </div>
                    </div>
                </div>
            </div>

            {/* Main Content Layout */}
            <div className="max-w-6xl mx-auto px-10 pt-40 pb-24 flex flex-col lg:flex-row gap-20">
                <div className="flex-grow">
                    <h2 className="text-2xl font-black text-gray-900 uppercase mb-8 tracking-tight flex items-center gap-3">
                        About the movie
                        <div className="h-1 w-12 bg-[#F84464] rounded-full"></div>
                    </h2>
                    <p className="text-gray-500 leading-relaxed text-lg mb-10 font-medium">
                        Experience the cinematic brilliance of <span className="text-black font-bold">{movie.movieName}</span>. 
                        As a top-rated <span className="italic text-[#F84464]">{movie.genre}</span> film, it delivers an 
                        emotional and visual journey that has captivated audiences nationwide. 
                        Don't miss out on this spectacular experience in <span className="font-bold">{movie.language}</span>.
                    </p>
                    
                    <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
                        <div className="p-6 bg-gray-50 rounded-3xl border border-gray-100 flex items-center gap-4">
                            <div className="w-10 h-10 bg-white rounded-xl flex items-center justify-center shadow-sm">🌐</div>
                            <div>
                                <p className="text-[10px] font-black text-gray-400 uppercase tracking-widest">Language</p>
                                <p className="font-bold text-gray-800">{movie.language}</p>
                            </div>
                        </div>
                        <div className="p-6 bg-gray-50 rounded-3xl border border-gray-100 flex items-center gap-4">
                            <div className="w-10 h-10 bg-white rounded-xl flex items-center justify-center shadow-sm">🕒</div>
                            <div>
                                <p className="text-[10px] font-black text-gray-400 uppercase tracking-widest">Duration</p>
                                <p className="font-bold text-gray-800">{movie.durationMinutes} Minutes</p>
                            </div>
                        </div>
                    </div>
                </div>

                <div className="w-full lg:w-80 shrink-0">
                    <div className="bg-white p-8 rounded-[2.5rem] shadow-2xl border border-gray-50 sticky top-10">
                        <div className="mb-10 text-center lg:text-left">
                            <p className="text-[10px] font-black text-gray-300 uppercase tracking-[0.2em] mb-3">Ticket Price</p>
                            <p className="text-4xl font-black text-gray-900">
                                ₹{180 + (movie.movieId * 10)} 
                                <span className="text-xs font-bold text-gray-400 ml-2 uppercase tracking-tighter">onwards</span>
                            </p>
                        </div>
                        
                        <button 
                            onClick={() => navigate(`/booking/${movie.movieId}`)} 
                            className="w-full bg-[#F84464] text-white py-5 rounded-2xl font-black text-sm uppercase tracking-widest hover:bg-[#d63a56] transition-all shadow-xl hover:shadow-[#f8446444] active:scale-95 mb-6"
                        >
                            Book Tickets
                        </button>
                        
                        <div className="flex items-center justify-center gap-2 text-[10px] text-gray-400 font-bold uppercase tracking-tighter">
                            <span className="text-green-500">✔</span> Flexible Cancellation
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default MovieDetails;