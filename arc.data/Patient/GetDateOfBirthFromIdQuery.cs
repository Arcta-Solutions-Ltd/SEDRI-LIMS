using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Patient
{
    /// <summary>
    /// Retrieves a patient's date of birth by patient ID for specimen age calculation.
    /// </summary>
    internal class GetDateOfBirthFromIdQuery : IQueryReturningString
    {
        /// <summary>
        /// Executes the query to fetch date of birth.
        /// </summary>
        /// <param name="connect">Database connection.</param>
        /// <param name="queryFilters">Query parameters containing patient id.</param>
        /// <returns>Date of birth as ISO string (yyyy-MM-dd), or null if not set.</returns>
        public async Task<string> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.Parameters.First(p => p.Key.Equals("id", StringComparison.OrdinalIgnoreCase));
            var sql = @"SELECT dateofbirth FROM patient WHERE id = @Id;";
            var result = await connect.QueryFirstOrDefaultAsync<DateTime?>(sql, new { Id = int.Parse(id.Value) });
            return result.HasValue ? result.Value.ToString("yyyy-MM-dd") : null;
        }
    }
}
