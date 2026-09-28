using arc.common.Models.Export;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Exports
{
    internal class ExportProfileListQuery : IQueryReturningType<List<ExportProfileModel>>
    {
        public async Task<List<ExportProfileModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var sql = @"select Id As Key, Name As Text, Description As Text, ModifiedDate As Date, Enabled As Boolean from exportprofile order by Id";

            var result = await connect.QueryAsync<ExportProfileModel>(sql);

            return result.ToList();
        }
    }
}
