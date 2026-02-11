import axiosInstance from './axiosInstance';

export const userService = {
    register: async (userData) => {
        const response = await axiosInstance.post('/Users', userData);
        return response.data;
    },

    login: async (email, password) => {
        // Your backend uses paged responses
        const response = await axiosInstance.get('/Users/paged?pageSize=100');
        
        console.log("User List Response:", response.data);

        // The actual array of users is inside response.data.data
        const userList = response.data.data || [];
        
        // Match against your database fields: email and passwordHash
        const user = userList.find(u => u.email === email && u.passwordHash === password);
        
        return user;
    }
};