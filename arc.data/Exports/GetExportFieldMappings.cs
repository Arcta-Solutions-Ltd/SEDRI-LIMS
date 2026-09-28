using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Exports;
internal class GetExportFieldMappings : IQueryReturningType<List<string>>
{
    public async Task<List<string>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var sql = @"select moredata::jsonb->>'Mapping' as Mapping from exportprofilerecord";

        var exportProfile = await connect.QueryAsync<string>(sql);

        var mappingList = exportProfile.ToList();
        mappingList.RemoveAll(p => p == null);

        return mappingList;
    }
}
