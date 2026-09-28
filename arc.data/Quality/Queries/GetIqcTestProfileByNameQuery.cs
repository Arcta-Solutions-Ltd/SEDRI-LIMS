using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Quality;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Quality.Queries
{
    internal class GetIqcTestProfileByNameQuery : IQueryReturningType<IqcTestProfile>
    {
        public async Task<IqcTestProfile> ExecuteAsync(NpgsqlConnection connection, QueryFilterConfig queryFilters)
        {
            var name = queryFilters.GetStringValue("Name");
            var testMethodId = queryFilters.GetIntegerValue("TestMethodId");

            var sql = @"SELECT itp.* FROM iqctestprofiles itp
                    LEFT JOIN listitem li on li.id = itp.testmethodlistitemid
					LEFT JOIN list l on l.id = li.listid
                    WHERE UPPER(itp.name) = UPPER(@name) and li.id = @testMethodId and itp.deleteddate is null";

            return await connection.QuerySingleOrDefaultAsync<IqcTestProfile>(sql, new { name, testMethodId });
        }
    }
}
