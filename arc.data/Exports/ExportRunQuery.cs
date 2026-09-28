using arc.common.Utils;
using arc.data.Organisation;
using arc.data.Utils;
using arc.domain.Configuration.PagesConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Exports;

public class ExportRunQuery
{
    public async Task<List<string>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters, List<FieldConfig> fieldConfigs, IJsonReplacer jsonReplacer)
    {
        _ = queryFilters.TryParseDateValue("startdate", out var startDate, new DateTime());
        _ = queryFilters.TryParseDateValue("enddate", out var endDate, new DateTime());

        var astExclusive = queryFilters.GetStringValue("astExclusive");

        var containsASTFields = fieldConfigs.Any(r => r.TableName.ToLower() == "ast");

        var containsCultureFields = fieldConfigs.Any(r => r.TableName.ToLower() == "culture" || r.TableName.ToLower() == "culturetests") || containsASTFields;

        if (!containsCultureFields)
        {
            queryFilters.Null("organismid");
        }

        if(queryFilters.Parameters.Any(p => p.Key.Equals("organisationid", StringComparison.OrdinalIgnoreCase)))
        {
            var orgQuery = new OrganisationHierarchyListQuery();
            var tempOrganisationList = await orgQuery.ExecuteAsync(connect, queryFilters);
            queryFilters.Parameters.FirstOrDefault(p => p.Key.Equals("organisationid", StringComparison.OrdinalIgnoreCase)).Value = tempOrganisationList;
        }

        var dateFieldName = queryFilters.GetStringValue("datefieldname");
        if (string.IsNullOrWhiteSpace(dateFieldName))
        {
            dateFieldName = "s.CollectionDate";
        }
        var whereClause = await SpecimenFilter.GetWhereClauseAsync(queryFilters, connect, dateFieldName);
        var join = await SpecimenFilter.GetJoinClauseAsync(queryFilters, connect);
        var with = "";

        var containsOrganism = fieldConfigs.Any(r => r.Id == "SpecimenOrganism" || r.Id == "OrganismNameOrGrowth" || r.Id == "OrganismPreferredName");
        if (containsOrganism)
        {
            with += with == "" ? " with " : ", ";
            with += @"organisms as (select o.Id, TRIM(CONCAT(g.name,case when g.name is not null and s.name is null and a.name is null then ' spp.' else '' end, ' ',s.name,' ',ss.name,' ',TRIM(se.name), a.name)) as organismname
                        from organism o
                        left outer join genus g on g.Id = o.genusId
                        left outer join species s on s.Id = o.speciesId
                        left outer join subspecies ss on ss.id = o.subspeciesId
                        left outer join serotype se on se.Id = o.serotypeId
                        left outer join additional a on a.Id = o.additionalId)";
        }

        if (fieldConfigs.Any(r => r.TableName.ToLower() == "tests"))
        {
            with += with == "" ? " with " : ", ";
            with += @"testres as (select t.specimenid, r.* from tests t, jsonb_each_text(testresults) r) ";
        }
        if (fieldConfigs.Any(r => r.TableName.ToLower() == "culturetests"))
        {
            with += with == "" ? " with " : ", ";
            with += @"culturetestres as (select t.cultureid, r.* from culturetests t, jsonb_each_text(testresults) r) ";
        }
        if (fieldConfigs.Any(r => r.TableName.ToLower() == "specimen"))
        {
            with += with == "" ? " with " : ", ";
            with += @"specmoredata as (select t.id, r.* from specimen t, jsonb_each_text(moredata) r)";
        }
        if (fieldConfigs.Any(r => r.TableName.ToLower() == "patient"))
        {
            with += with == "" ? " with " : ", ";
            with += @"patientmoredata as (select t.id, r.* from patient t, jsonb_each_text(moredata) r)";
        }
        if (fieldConfigs.Any(r => r.Id.ToLower() == "reportdate"))
        {
            with += with == "" ? " with " : ", ";
            with += @"reportdateres as (select specimenid, max(lastmodifieddate) from reporthistory group by specimenid)";
        }

        var sqlBuilder = new ExportFieldBuilder();
        sqlBuilder.BuildSql(fieldConfigs, jsonReplacer);

        var sql = with + @" select " + sqlBuilder.GetSqlString() + @" from specimen s ";
        if (containsCultureFields && queryFilters.Parameters.FirstOrDefault(p => p.Key.ToLower() == "organismid")?.Value == null)
        {
            sql += "left join culture c on s.id = c.specimenid ";
        }
        if (containsASTFields)
        {
            sql += astExclusive == "Yes" ? "inner join ast ast on c.id = ast.cultureid " : "left join ast ast on c.id = ast.cultureid ";
        }

        sql += join + sqlBuilder.GetJoins() + " " + whereClause;

        var result = await connect.QueryAsync<string>(sql, new { startDate, endDate });

        return result.ToList();
    }
}

