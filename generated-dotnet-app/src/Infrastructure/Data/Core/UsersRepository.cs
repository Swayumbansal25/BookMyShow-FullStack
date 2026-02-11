using BookMyShow.Core.Entities.Core;
using BookMyShow.Core.Interfaces.Core;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookMyShow.Infrastructure.Data.Common;

namespace BookMyShow.Infrastructure.Data.Core
{
    public class UsersRepository : IUsersRepository
    {
        private readonly string _connectionString;

        public UsersRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Users?> GetByIdAsync(long id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Users>(
                $"SELECT * FROM users WHERE user_id = @id", 
                new { id });
        }

        public async Task<IEnumerable<Users>> GetAllAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryAsync<Users>(
                $"SELECT * FROM users");
        }

        public async Task<(IEnumerable<Users> Items, int TotalCount)> GetPagedAsync(
            int pageNumber, 
            int pageSize, 
            string? searchTerm = null,
            string? sortBy = null,
            bool sortDescending = false,
            Dictionary<string, object>? filters = null)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var allowedColumns = new[] { "user_id", "full_name", "email", "phone_number", "password_hash", "date_of_birth", "gender", "is_verified", "is_active", "created_at", "updated_at" };

            var builder = SqlQueryBuilder
                .From("users", allowedColumns)
                .SearchAcross(new string[] { "full_name", "email", "phone_number", "password_hash", "gender",  }, searchTerm)
                .ApplyFilters(filters)
                .Paginate(pageNumber, pageSize);

            var allowedSet = new HashSet<string>(allowedColumns, System.StringComparer.OrdinalIgnoreCase);
            var orderBy = !string.IsNullOrWhiteSpace(sortBy) && allowedSet.Contains(sortBy!) ? sortBy! : "user_id";
            builder.OrderBy(orderBy, sortDescending);

            var (dataSql, parameters) = builder.BuildSelect();
            var (countSql, _) = builder.BuildCount();

            using var multi = await connection.QueryMultipleAsync(dataSql + ";" + countSql, parameters);
            var items = await multi.ReadAsync<Users>();
            var totalCount = await multi.ReadSingleAsync<int>();
            return (items, totalCount);
        }

        public async Task<Users> AddAsync(Users entity)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var sql = @"INSERT INTO users 
                (full_name, email, phone_number, password_hash, date_of_birth, gender, is_verified, is_active, created_at, updated_at)
                VALUES
                (@FullName, @Email, @PhoneNumber, @PasswordHash, @DateOfBirth, @Gender, @IsVerified, @IsActive, NOW(), NOW())
                RETURNING *";
            
            var createdEntity = await connection.QuerySingleAsync<Users>(sql, entity);
            return createdEntity;
        }

        public async Task UpdateAsync(Users entity)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var sql = @"UPDATE users SET
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
           
                
                
                full_name = @FullName,
                email = @Email,
                phone_number = @PhoneNumber,
                password_hash = @PasswordHash,
                date_of_birth = @DateOfBirth,
                gender = @Gender,
                is_verified = @IsVerified,
                is_active = @IsActive,
                updated_at = NOW()
                WHERE user_id = @UserId";
            await connection.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(long id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                $"DELETE FROM users WHERE user_id = @id", 
                new { id });
        }
    }
}