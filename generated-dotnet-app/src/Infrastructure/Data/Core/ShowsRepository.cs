using BookMyShow.Core.Entities.Core;
using BookMyShow.Core.Interfaces.Core;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMyShow.Infrastructure.Data.Core
{
    public class ShowsRepository : IShowsRepository
    {
        private readonly string _connectionString;

        public ShowsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // ---------------- GET BY ID ----------------
        public async Task<Show?> GetByIdAsync(long showId)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    show_id,
                    movie_id,
                    screen_id,
                    show_date,
                    start_time,
                    end_time,
                    price
                FROM shows
                WHERE show_id = @Id";

            var row = await conn.QuerySingleOrDefaultAsync<dynamic>(sql, new { Id = showId });

            if (row == null)
                return null;

            return MapShow(row);
        }

        // ---------------- GET PAGED ----------------
        public async Task<(IEnumerable<Show>, int)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            long? movieId,
            long? screenId)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            var where = "WHERE 1=1";
            var parameters = new DynamicParameters();

            if (movieId.HasValue)
            {
                where += " AND movie_id = @MovieId";
                parameters.Add("MovieId", movieId);
            }

            if (screenId.HasValue)
            {
                where += " AND screen_id = @ScreenId";
                parameters.Add("ScreenId", screenId);
            }

            var offset = (pageNumber - 1) * pageSize;

            var dataSql = $@"
                SELECT 
                    show_id,
                    movie_id,
                    screen_id,
                    show_date,
                    start_time,
                    end_time,
                    price
                FROM shows
                {where}
                ORDER BY show_id
                LIMIT @Limit OFFSET @Offset";

            var countSql = $"SELECT COUNT(*) FROM shows {where}";

            parameters.Add("Limit", pageSize);
            parameters.Add("Offset", offset);

            using var multi = await conn.QueryMultipleAsync(dataSql + ";" + countSql, parameters);

            var rows = await multi.ReadAsync<dynamic>();
            var total = await multi.ReadSingleAsync<int>();

            var data = rows.Select<dynamic, Show>(r => MapShow(r)).ToList();

            return (data, total);
        }

        // ---------------- ADD ----------------
        public async Task<Show> AddAsync(Show show)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            var sql = @"
                INSERT INTO shows 
                (movie_id, screen_id, show_date, start_time, end_time, price)
                VALUES
                (@MovieId, @ScreenId, @ShowDate, @StartTime, @EndTime, @Price)
                RETURNING show_id";

            var id = await conn.ExecuteScalarAsync<long>(sql, new
            {
                show.MovieId,
                show.ScreenId,
                ShowDate = show.ShowDate.ToDateTime(TimeOnly.MinValue),
                StartTime = show.StartTime.ToTimeSpan(),
                EndTime = show.EndTime.ToTimeSpan(),
                show.Price
            });

            show.ShowId = id;
            return show;
        }

        // ---------------- UPDATE ----------------
        public async Task UpdateAsync(Show show)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            var sql = @"
                UPDATE shows SET
                    movie_id = @MovieId,
                    screen_id = @ScreenId,
                    show_date = @ShowDate,
                    start_time = @StartTime,
                    end_time = @EndTime,
                    price = @Price
                WHERE show_id = @Id";

            await conn.ExecuteAsync(sql, new
            {
                Id = show.ShowId,
                show.MovieId,
                show.ScreenId,
                ShowDate = show.ShowDate.ToDateTime(TimeOnly.MinValue),
                StartTime = show.StartTime.ToTimeSpan(),
                EndTime = show.EndTime.ToTimeSpan(),
                show.Price
            });
        }

        // ---------------- DELETE ----------------
        public async Task DeleteAsync(long showId)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            var sql = "DELETE FROM shows WHERE show_id = @Id";
            await conn.ExecuteAsync(sql, new { Id = showId });
        }

        // ---------------- MAPPER ----------------
        private static Show MapShow(dynamic r)
        {
            return new Show
            {
                ShowId = (long)r.show_id, // Updated from r.id
                MovieId = (long)r.movie_id,
                ScreenId = (long)r.screen_id,
                ShowDate = DateOnly.FromDateTime((DateTime)r.show_date),
                StartTime = TimeOnly.FromTimeSpan((TimeSpan)r.start_time),
                EndTime = TimeOnly.FromTimeSpan((TimeSpan)r.end_time),
                Price = (decimal)r.price
            };
        }
    }
}