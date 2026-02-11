using System.Collections.Generic;

namespace BookMyShow.WebApi.DTOs.Core
{
    public class TheatresFilterRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public long? CityId { get; set; }

        public Dictionary<string, object> GetFilters()
        {
            var dict = new Dictionary<string, object>();
            if (CityId.HasValue) dict.Add("city_id", CityId.Value);
            return dict;
        }
    }
}
