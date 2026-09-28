using arc.common.Models.Export;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Exports;

public static class CommentExportQuery
{
    public static async Task<List<ExportComment>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        _ = queryFilters.TryParseDateValue("startdate", out var startDate, new DateTime());
        _ = queryFilters.TryParseDateValue("enddate", out var endDate, new DateTime());

        var whereClause = await SpecimenFilter.GetWhereClauseAsync(queryFilters, connect);

        var sql = $"""
            select
                spc.specimenid,
                spc.cultureid,
                spc.commenttypeid,
                spc.comment,
                li.value as cannedcomment
            from
                specimencomment spc
                left outer join listitem li on li.id = spc.cannedcommentid
                left outer join specimen s on s.id = spc.specimenid
            {whereClause}
            """;

        var result = await connect.QueryAsync<ExportComment>(sql, new { startDate, endDate });

        return [.. result];
    }
}
