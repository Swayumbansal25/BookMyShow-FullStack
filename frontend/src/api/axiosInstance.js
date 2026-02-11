import axios from 'axios';

const axiosInstance = axios.create({
    // This must match your .NET Backend port from Program.cs
    baseURL: 'http://localhost:5000/api/Core', 
    headers: {
        'Content-Type': 'application/json'
    }
});

export default axiosInstance;