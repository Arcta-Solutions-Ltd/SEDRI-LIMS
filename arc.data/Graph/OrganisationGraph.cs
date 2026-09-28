using arc.common.Models.Graph;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Graph;

internal class OrganisationGraph : IQueryReturningType<List<GraphModel>>
{
    public async Task<List<GraphModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var dateSelector = new DateSelection();
        dateSelector.IncorporateDateInterval(queryFilters, "organisationid", "o.id, o.fullyqualifiedname as value");

        _ = queryFilters.TryParseDateValue("startdate", out var startDate, new DateTime());
        _ = queryFilters.TryParseDateValue("enddate", out var endDate, new DateTime());

        var whereClause = await SpecimenFilter.GetWhereClauseAsync(queryFilters, connect);
        var join = await SpecimenFilter.GetJoinClauseAsync(queryFilters, connect);

        var sql = $"""
            with tablevalues as
                (
                    select s.organisationid, count(*) as number, {dateSelector.Select}
                    from specimen s
                    {join}
                    {whereClause}
                    group by {dateSelector.GroupBy}
                )
            select {dateSelector.TableSelection} from tablevalues tv
            inner join organisation o on tv.organisationid = o.id
            order by {dateSelector.OrderBy}
            """;

        var result = await connect.QueryAsync<GraphModel>(sql, new { startDate, endDate });

        return MakeSureCorrectOrganisationLevelsAreDisplayed(result, queryFilters);
    }

    private static List<GraphModel> MakeSureCorrectOrganisationLevelsAreDisplayed(IEnumerable<GraphModel> rows, QueryFilterConfig queryFilters)
    {
        var queryFilterHasOrganisationLevelId = queryFilters.TryParseIntegerValue("organisationlevelid", out var organisationLevelId);

        var organisationLevel = queryFilterHasOrganisationLevelId ? organisationLevelId : 1191;
        var levelToDisplay = organisationLevel - 1191;

        //Get the hierarchy elements to check
        var returnList = TotalNumberOfMatchesIncorporatingTheHierarchy.FilterRowsBasedOnLevel(rows, levelToDisplay);

        foreach (var row in returnList)
        {
            row.Value = TotalNumberOfMatchesIncorporatingTheHierarchy.ReplaceWithLastElement(row.Value);
        }

        return returnList;
    }
}
