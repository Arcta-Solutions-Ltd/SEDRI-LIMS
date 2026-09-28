using arc.common.Models.Export;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Exports;

public static class AstExportQuery
{
    public static async Task<List<WhonetAntibiotic>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        _ = queryFilters.TryParseDateValue("startdate", out var startDate, new DateTime());
        _ = queryFilters.TryParseDateValue("enddate", out var endDate, new DateTime());

        var whereClause = await SpecimenFilter.GetWhereClauseAsync(queryFilters, connect);
        var join = await SpecimenFilter.GetJoinClauseAsync(queryFilters, connect);

        if (queryFilters.TryGetStringValue("antibiotics", out var antibioticsIdCsv))
        {
            whereClause += $"{Environment.NewLine}and ast.antibioticid in ({antibioticsIdCsv})";
        }

        var sql = $"""
            select
                cultureid,
                ant.antibioticname as antibiotic,
                ant.id as antibioticid,
                ant.code as antibioticcode,
                li1.value as susceptibility,
                ast.testmethodid,
                ast.dosage,
                ast.guidelinesid,
                ast.measurement,
                ast.miccomparison
            from
                ast
                inner join culture cul on cul.id = ast.cultureid
                inner join specimen s on s.id = cul.specimenid
                inner join antibiotic ant on ant.id = ast.antibioticid
                left outer join listitem li1 on li1.id = ast.susceptibilityid
            {join}
            {whereClause}
            order by cul.id
            """;

        var result = await connect.QueryAsync<WhonetAntibiotic>(sql, new { startDate, endDate });

        return [.. result];
    }
}
