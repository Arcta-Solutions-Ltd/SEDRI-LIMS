using arc.common.Models.Language;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Configuration
{
    /// <summary>
    /// Represents a query to retrieve a language by translation ID.
    /// </summary>
    internal class GetLanguageByTranslationIdQuery : IQueryReturningType<LanguageModel>
    {
        /// <summary>
        /// Executes the query asynchronously to retrieve a language based on the specified query filters.
        /// </summary>
        /// <param name="connect">The Npgsql connection to use for the query.</param>
        /// <param name="queryFilters">The query filter configuration.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the language model.</returns>
        public async Task<LanguageModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var translationId = queryFilters.Parameters
                .FirstOrDefault(p => p.Key.Equals("translationid", StringComparison.OrdinalIgnoreCase))?.Value;

            if (translationId == null)
            {
                throw new ArgumentException("TranslationId is missing in query filters.");
            }

            var sql = "SELECT * FROM language WHERE TranslationId = @TranslationId";

            var result = await connect.QueryFirstOrDefaultAsync<LanguageModel>(sql, new { TranslationId = int.Parse(translationId) });

            return result;
        }
    }
}
