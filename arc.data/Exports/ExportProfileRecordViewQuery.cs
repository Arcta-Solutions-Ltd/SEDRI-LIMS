using arc.common.Models.Export;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Exports;
internal class ExportProfileRecordViewQuery : IQueryReturningType<List<ExportProfileFieldModel>>
{
    public async Task<List<ExportProfileFieldModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var id = queryFilters.Parameters.FirstOrDefault(p => p.Key.ToLower() == "exportprofileid");

        var sql = @"select e.*, c.contents::jsonb->>'Name' as Mapping from exportprofilerecord e left outer join Configs c on c.configname = e.moredata::jsonb->>'Mapping' where e.exportprofileid = @Id ORDER BY e.ordernumber";

        var exportProfileFields = await connect.QueryAsync<ExportProfileFieldModel>(sql, new { Id = int.Parse(id.Value) });

        return exportProfileFields.ToList();
    }
}
