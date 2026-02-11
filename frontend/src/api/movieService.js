import axiosInstance from './axiosInstance';

export const movieService = {
    getAllMovies: async () => {
        try {
            const response = await axiosInstance.get('/Movies/paged?pageSize=100');
            
            // Your log shows the movies are in response.data.data
            console.log("Full Backend Response:", response.data);
            
            // Extracting from 'data' instead of 'items'
            return response.data.data || []; 
        } catch (error) {
            console.error("Error in getAllMovies:", error);
            return [];
        }
    },
    getMovieById: async (id) => {
        const response = await axiosInstance.get(`/Movies/${id}`);
        // Based on your controller, this might also be response.data.value or just response.data
        return response.data;
    }
};