using arc.common.Models.Export;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Exports;

/// <summary>
/// Retrieves a single export history record by id for the record view.
/// </summary>
internal class ExportHistoryByIdQuery : IQueryReturningType<ExportHistoryModel>
{
    public async Task<ExportHistoryModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var id = queryFilters.GetIntegerValue("id");
        var sql = @"
            SELECT erh.id, erh.exportprofileid, ep.name AS exportprofilename, erh.runat, erh.filter, erh.fileattachmentid,
                erh.exportscheduleid, es.name AS schedulename
            FROM exportrunhistory erh
            LEFT JOIN exportprofile ep ON ep.id = erh.exportprofileid
            LEFT JOIN exportschedule es ON es.id = erh.exportscheduleid
            WHERE erh.id = @id";

        return await connect.QueryFirstOrDefaultAsync<ExportHistoryModel>(sql, new { id });
    }
}
