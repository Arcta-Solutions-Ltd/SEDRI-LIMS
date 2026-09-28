using arc.common.Models.Graph;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Graph;

internal class LocationGraph : IQueryReturningType<List<GraphModel>>
{
    public async Task<List<GraphModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var dateSelector = new DateSelection();
        dateSelector.IncorporateDateInterval(queryFilters, "locationid", "lo.id, lo.fullyqualifiedname as value");

        _ = queryFilters.TryParseDateValue("startdate", out var startDate, new DateTime());
        _ = queryFilters.TryParseDateValue("enddate", out var endDate, new DateTime());

        var whereClause = await SpecimenFilter.GetWhereClauseAsync(queryFilters, connect);
        var join = await SpecimenFilter.GetJoinClauseAsync(queryFilters, connect);

        var sql = $"""
            with tablevalues as
                (
                    select p.locationId, count(*) as number, {dateSelector.Select}
                    from specimen s 
                    {join}
                    {whereClause}
                    group by {dateSelector.GroupBy}
                )
            select {dateSelector.TableSelection} from tablevalues tv
            inner join location lo on tv.locationid = lo.id
            order by {dateSelector.OrderBy}
            """;

        var result = await connect.QueryAsync<GraphModel>(sql, new { startDate, endDate });

        return MakeSureCorrectLocationLevelsAreDisplayed(result, queryFilters);
    }

    private static List<GraphModel> MakeSureCorrectLocationLevelsAreDisplayed(IEnumerable<GraphModel> rows, QueryFilterConfig queryFilters)
    {
        var queryFiltersHasLocationLevelId = queryFilters.TryParseIntegerValue("locationlevelid", out var locationLevelId);

        var locationLevel = queryFiltersHasLocationLevelId ? locationLevelId : 1191;
        var levelToDisplay = locationLevel - 1191;

        //Get the hierarchy elements to check
        var returnList = TotalNumberOfMatchesIncorporatingTheHierarchy.FilterRowsBasedOnLevel(rows, levelToDisplay);

        foreach (var row in returnList)
        {
            row.Value = TotalNumberOfMatchesIncorporatingTheHierarchy.ReplaceWithLastElement(row.Value);
        }

        return returnList;
    }
}
