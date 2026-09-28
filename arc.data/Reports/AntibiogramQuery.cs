using arc.common.Models.Reports;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Reports;

/// <summary>
/// A query class that retrieves a list of antibiograms based on provided query filters.
/// </summary>
internal class AntibiogramQuery : IQueryReturningType<List<AntibiogramModel>>
{
    /// <summary>
    /// Executes the query asynchronously to fetch antibiogram data.
    /// </summary>
    /// <param name="connect">An open NpgsqlConnection to the database.</param>
    /// <param name="queryFilters">The filters to apply to the query, such as susceptibility ID, start date, and end date.</param>
    /// <returns>A list of antibiogram models that match the query.</returns>
    public async Task<List<AntibiogramModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var susceptibilityId = queryFilters.GetIntegerValue("susceptibilityid");

        _ = queryFilters.TryParseDateValue("startdate", out var startDate, new DateTime());
        _ = queryFilters.TryParseDateValue("enddate", out var endDate, new DateTime());

        var whereClause = await SpecimenFilter.GetWhereClauseAsync(queryFilters, connect);

        var sql = $"""
            with
                tab as (
                    select
                        ast.antibioticid,
                        c.specimenorganismid as organismid,
                        count(*) as total,
                        sum(
                            case
                                when ast.susceptibilityid = @susceptibilityId then 1
                                else 0
                            end
                        ) as resistant
                    from
                        specimen s
                        inner join patient p on s.patientid = p.id
                        inner join culture c on s.id = c.specimenid
                        inner join ast on c.id = ast.cultureid
                        {whereClause}
                    group by
                        ast.antibioticid,
                        c.specimenorganismid
                )
            select
                ant.antibioticname,
                tab.antibioticid,
                tab.organismid,
                trim(
                    concat (
                        g.name,
                        case
                            when g.name is not null
                            and s.name is null
                            and a.name is null then ' spp.'
                            else ''
                        end,
                        ' ',
                        s.name,
                        ' ',
                        ss.name,
                        ' ',
                        trim(se.name),
                        a.name
                    )
                ) as organismname,
                tab.total,
                tab.resistant,
                (tab.resistant/tab.total::float) * 100 as percentage
            from
                tab
                inner join antibiotic ant on ant.id = tab.antibioticid
                inner join organism o on o.id = tab.organismid
                left outer join genus g on g.id = o.genusid
                left outer join species s on s.id = o.speciesid
                left outer join subspecies ss on ss.id = o.subspeciesid
                left outer join serotype se on se.id = o.serotypeid
                left outer join additional a on a.id = o.additionalid
            where
                tab.resistant > 0
            """;

        var result = await connect.QueryAsync<AntibiogramModel>(sql, new { susceptibilityId, startDate, endDate });
        return [.. result];
    }
}

