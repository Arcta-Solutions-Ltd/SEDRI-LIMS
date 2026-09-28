using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.SystemConfig
{
    /// <summary>
    /// Checks the configs table to make sure the name being suggested has not been used. If it has then a new unused name is suggested.
    /// </summary>
    internal class NextFreeNameQuery : IQueryReturningString
    {
        /// <summary>
        /// Checks the configs table to make sure the name being suggested has not been used. If it has then a new unused name is suggested.
        /// </summary>
        /// <param name="connect">Database connection</param>
        /// <param name="queryFilters">Set of filter conditions which are passed to the query. The filters that must be passed are:
        /// 1. Name - the name of the entry in the config table
        /// 2. Suffix - Any suffix that must be added to the name before it is used in the configs table
        /// </param>
        /// <returns>Returns a string representing the next available name</returns>
        public async Task<string> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            return await ExecuteAsync(connect, queryFilters, null);
        }

        /// <summary>
        /// Checks the configs table to make sure the name being suggested has not been used. If it has then a new unused name is suggested.
        /// Names are unique across every configuration type, because configuration records are looked up by name alone
        /// in several places, so this deliberately does not filter by ConfigTypeId.
        /// </summary>
        /// <param name="connect">Database connection</param>
        /// <param name="queryFilters">Set of filter conditions which are passed to the query. The filters that must be passed are:
        /// 1. Name - the name of the entry in the config table
        /// 2. Suffix - Any suffix that must be added to the name before it is used in the configs table
        /// </param>
        /// <param name="transaction">Transaction to run the lookups in, so the name reservation sees uncommitted inserts made earlier in the same unit of work.</param>
        /// <returns>Returns a string representing the next available name</returns>
        public async Task<string> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters, IDbTransaction transaction)
        {
            var name = queryFilters.Parameters.First(p => p.Key.ToLower() == "name");
            var suffix = queryFilters.Parameters.First(p => p.Key.ToLower() == "suffix");
            var returnName = name.Value + suffix.Value;
            var letter = (char)('a'- 1);

            var sql = @"select count(*) from configs where Lower(ConfigName) = Lower(@Name)";
            var result = await connect.QueryFirstAsync<long>(sql, new { Name = returnName }, transaction);

            while (result > 0)
            {
                letter = GetNextLetter(letter);
                returnName += letter;
                result = await connect.QueryFirstAsync<long>(sql, new { Name = returnName }, transaction);
            };

            return returnName.ToLower();
        }

        private static char GetNextLetter(char current)
        {
            return (char)(current + 1);
        }
    }
}
