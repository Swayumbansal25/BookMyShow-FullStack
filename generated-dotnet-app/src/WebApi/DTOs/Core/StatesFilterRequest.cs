using System.Collections.Generic;

namespace BookMyShow.WebApi.DTOs.Core
{
    public class StatesFilterRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }

        public string? StateName { get; set; }

        public Dictionary<string, object> GetFilters()
        {
            var filters = new Dictionary<string, object>();

            if (!string.IsNullOrWhiteSpace(StateName))
                filters.Add("state_name", StateName);

            return filters;
        }
    }
}
