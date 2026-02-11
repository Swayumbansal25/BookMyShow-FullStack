using System;
using System.Collections.Generic;

namespace BookMyShow.WebApi.DTOs.Core
{
    public class UsersFilterRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;
        
        
        
        public string? FullName { get; set; }
        
        
        
        public string? Email { get; set; }
        
        
        
        public string? PhoneNumber { get; set; }
        
        
        
        public string? PasswordHash { get; set; }
        
        
        
        
        
        public string? Gender { get; set; }
        
        
        
        
        
        
        
        
        
        
        
        public Dictionary<string, object> GetFilters()
        {
            var filters = new Dictionary<string, object>();
            
            
            
            if (FullName != null && !string.IsNullOrWhiteSpace(FullName))
                filters.Add("full_name", FullName);
            
            
            
            if (Email != null && !string.IsNullOrWhiteSpace(Email))
                filters.Add("email", Email);
            
            
            
            if (PhoneNumber != null && !string.IsNullOrWhiteSpace(PhoneNumber))
                filters.Add("phone_number", PhoneNumber);
            
            
            
            if (PasswordHash != null && !string.IsNullOrWhiteSpace(PasswordHash))
                filters.Add("password_hash", PasswordHash);
            
            
            
            
            
            if (Gender != null && !string.IsNullOrWhiteSpace(Gender))
                filters.Add("gender", Gender);
            
            
            
            
            
            
            
            
            
            
            
            return filters;
        }
    }
}