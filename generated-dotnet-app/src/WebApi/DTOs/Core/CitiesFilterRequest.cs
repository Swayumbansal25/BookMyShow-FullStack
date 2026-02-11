using System.Collections.Generic;

namespace BookMyShow.WebApi.DTOs.Core
{
    public class CitiesFilterRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public long? StateId { get; set; }

        public Dictionary<string, object> GetFilters()
        {
            var dict = new Dictionary<string, object>();
            if (StateId.HasValue) dict.Add("state_id", StateId.Value);
            return dict;
        }
    }
}
