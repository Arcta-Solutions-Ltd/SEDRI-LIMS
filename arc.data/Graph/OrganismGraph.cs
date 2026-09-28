using arc.common.Models.Graph;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Graph;

internal class OrganismGraph : IQueryReturningType<List<GraphModel>>
{
    public async Task<List<GraphModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var dateSelector = new DateSelection();
        var selectPortion = @"TRIM(CONCAT(g.name,case when g.name is not null and s.name is null and a.name is null then ' spp.' else '' end, ' ', s.name,' ', ss.name,' ', TRIM(se.name), a.name)) As Value";
        dateSelector.IncorporateDateInterval(queryFilters, "specimenorganismid", selectPortion);

        _ = queryFilters.TryParseDateValue("startdate", out var startDate, new DateTime());
        _ = queryFilters.TryParseDateValue("enddate", out var endDate, new DateTime());

        var whereClause = await SpecimenFilter.GetWhereClauseAsync(queryFilters, connect);
        var join = await SpecimenFilter.GetJoinClauseAsync(queryFilters, connect, false, true);

        var sql = $"""
            with tablevalues as
                (select c.specimenorganismId, count(*) as number, {dateSelector.Select}
                from specimen s {join} {whereClause}
                group by {dateSelector.GroupBy})
            select {dateSelector.TableSelection} from tablevalues tv
                inner join Organism o on tv.specimenorganismid = o.id
                left outer join genus g on g.Id = o.genusId
                left outer join species s on s.Id = o.speciesId
                left outer join subspecies ss on ss.id = o.subspeciesId
                left outer join serotype se on se.Id = o.serotypeId
                left outer join additional a on a.Id = o.additionalId
                order by {dateSelector.OrderBy};
            """;

        var result = await connect.QueryAsync<GraphModel>(sql, new { startDate, endDate });
        return result.ToList();
    }
}
