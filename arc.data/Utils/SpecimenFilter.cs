using arc.data.Coding;
using arc.data.Organisation;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.common.ExtensionMethods;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Utils;

/// <summary>
/// Provides methods to construct dynamic SQL WHERE clauses for specimen queries,
/// including laboratory, organisation, specimen type, state, gender, date range,
/// organisation hierarchy, and location hierarchy filters.
/// </summary>

internal static class SpecimenFilter
{
    /// <summary>
    /// Builds the SQL WHERE clause based on the supplied filter configuration and database connection.
    /// Supports hierarchical expansion for organisation and location filters, simple equality filters
    /// via <see cref="AddWhereClause"/>, and date range filters against the specified date field.
    /// </summary>
    /// <param name="queryFilters">
    /// Configuration object containing parameters for laboratoryid, organisationid,
    /// organisationfilterid, specimentypeid, locationid, stateid, genderid, startdate, and enddate.
    /// </param>
    /// <param name="connect">
    /// An open <see cref="NpgsqlConnection"/> used to resolve organisation and location hierarchies.
    /// </param>
    /// <param name="dateFieldName">
    /// The database column (including alias) to apply date range filters against.
    /// Defaults to "s.CollectionDate".
    /// </param>
    /// <returns>
    /// A string containing the assembled WHERE clause, including nested hierarchy filters
    /// and parameter placeholders (@startDate, @endDate). Returns an empty string if no filters apply.
    /// </returns>
    public static async Task<string> GetWhereClauseAsync(QueryFilterConfig queryFilters, NpgsqlConnection connect, string dateFieldName = "s.CollectionDate")
    {
        var laboratoryId = queryFilters.Parameters.Where(p => p.Key.Equals("laboratoryid", StringComparison.OrdinalIgnoreCase));
        var organisationId = queryFilters.Parameters.Where(p => p.Key.Equals("organisationid", StringComparison.OrdinalIgnoreCase));
        var organisationFilterId = queryFilters.Parameters.Where(p => p.Key.Equals("organisationfilterid", StringComparison.OrdinalIgnoreCase));
        var specimenTypeId = queryFilters.Parameters.Where(p => p.Key.Equals("specimentypeid", StringComparison.OrdinalIgnoreCase));
        var locationId = queryFilters.Parameters.Where(p => p.Key.Equals("locationid", StringComparison.OrdinalIgnoreCase));
        var stateId = queryFilters.Parameters.Where(p => p.Key.Equals("stateid", StringComparison.OrdinalIgnoreCase));
        var genderId = queryFilters.Parameters.Where(p => p.Key.Equals("genderid", StringComparison.OrdinalIgnoreCase));
        var queryFilterHasStartDate = queryFilters.TryParseDateValue("startdate", out var startDate);
        var queryFilterHasEndDate = queryFilters.TryParseDateValue("endDate", out var endDate);

        var whereClause = "";

        //Get Organisation Hierarchy

        var organisationFilter = "";
        if (organisationFilterId.Any() && !string.IsNullOrWhiteSpace(organisationFilterId.First().Value))
        {
            organisationFilter = organisationFilterId.First().Value;
        }
        var organisation = "";
        if (organisationId.Any() && !string.IsNullOrWhiteSpace(organisationId.First().Value))
        {
            organisation = organisationId.First().Value;
        }

        whereClause = await GetCorrectOrganisationFilterAsync(organisationFilter, organisation, connect, whereClause);

        whereClause = AddWhereClause(laboratoryId, whereClause, "LaboratoryId");
        whereClause = AddWhereClause(organisationId, whereClause, "OrganisationId");
        whereClause = AddWhereClause(specimenTypeId, whereClause, "specimenTypeId");
        whereClause = AddWhereClause(genderId, whereClause, "genderid", "p");
        whereClause = AddWhereClause(stateId, whereClause, "stateId");

        if (queryFilterHasStartDate)
        {
            whereClause += whereClause == "" ? "Where " : " and ";
            whereClause += " " + dateFieldName + " >= @startDate";
        }

        if (queryFilterHasEndDate)
        {
            whereClause += whereClause == "" ? "Where " : " and ";
            whereClause += " " + dateFieldName + " <= @endDate";
        }

        //Get Location Hierarchy
        if (locationId.Any())
        {
            var locationHierarchy = new LocationHierarchy();
            var locationIdList = locationId.First().Value;
            var locationList = "";
            if (locationId.Any() && !string.IsNullOrWhiteSpace(locationIdList))
            {
                var locationArray = locationIdList.Split(",");
                foreach (var location in locationArray)
                {
                    var newLocationList = await locationHierarchy.GetStringList(connect, int.Parse(location));
                    newLocationList = newLocationList == "" ? location : location + "," + newLocationList;
                    locationList += string.IsNullOrWhiteSpace(locationList) ? newLocationList : "," + newLocationList;
                }
                whereClause += whereClause == "" ? "Where " : " and ";
                whereClause += "p.LocationId in (" + SqlSanitizer.SanitizeIdList(locationList) + ") ";
            }
        }

        return whereClause;
    }

    /// <summary>
    /// Constructs the SQL JOIN clauses for patient, specimen tag, test, culture, and AST tables
    /// based on the provided filter parameters and include flags. Automatically expands organism
    /// filters into their full hierarchy via <see cref="CreateOrganismHierarchyAsync"/> when needed.
    /// </summary>
    /// <param name="queryFilters">
    /// Configuration object containing filter parameters:
    /// "tagid", "testid", "organismid", "susceptibilityid", and "antibioticid".
    /// </param>
    /// <param name="connect">
    /// An open <see cref="NpgsqlConnection"/> used to resolve organism hierarchies.
    /// </param>
    /// <param name="includeTagJoin">
    /// If true, always includes the SpecimenTag join even when no tag filter is supplied.
    /// </param>
    /// <param name="includeCultureJoin">
    /// If true, always includes the Culture join even when no organism filter is supplied.
    /// </param>
    /// <param name="includeAstJoin">
    /// If true, includes the AST join and applies susceptibility and antibiotic filters if present.
    /// </param>
    /// <param name="includeTestJoin">
    /// If true, always includes the Tests join even when no test filter is supplied.
    /// </param>
    /// <returns>
    /// A string containing the assembled INNER JOIN clauses based on the filters and flags.
    /// </returns>
    public static async Task<string> GetJoinClauseAsync(QueryFilterConfig queryFilters, NpgsqlConnection connect, bool includeTagJoin = false, bool includeCultureJoin = false, bool includeAstJoin = false, bool includeTestJoin = false)
    {
        var tagId = queryFilters.Parameters.Where(p => p.Key.Equals("tagid", StringComparison.OrdinalIgnoreCase));
        var testId = queryFilters.Parameters.Where(p => p.Key.Equals("testid", StringComparison.OrdinalIgnoreCase));
        var organismId = queryFilters.Parameters.Where(p => p.Key.Equals("organismid", StringComparison.OrdinalIgnoreCase));
        var susceptibilityId = queryFilters.Parameters.Where(p => p.Key.Equals("susceptibilityid", StringComparison.OrdinalIgnoreCase));
        var antibioticId = queryFilters.Parameters.Where(p => p.Key.Equals("antibioticid", StringComparison.OrdinalIgnoreCase));

        var join = "inner join Patient p on s.patientid = p.id ";
        var tagString = "";
        var testString = "";
        var organismString = "";

        if (tagId.Any() && !string.IsNullOrWhiteSpace(tagId.First().Value))
        {
            tagString = await CreateTagHierarchyAsync(queryFilters, connect);
            if (string.IsNullOrWhiteSpace(tagString))
            {
                tagString = tagId.First().Value;
            }

            join += "inner join SpecimenTag st on st.SpecimenId = s.Id and st.ListItemId in (" + SqlSanitizer.SanitizeIdList(tagString) + ") ";
        }
        else
        {
            if (includeTagJoin)
            {
                join += "inner join SpecimenTag st on st.SpecimenId = s.Id ";
            }
        }

        if ((testId.Any() && !string.IsNullOrWhiteSpace(testId.First().Value)) || includeTestJoin)
        {
            testString = SqlSanitizer.SanitizeStringList(string.Join(",", testId.First().Value));

            join += "inner join Tests tt on tt.SpecimenId = s.Id ";
            if (testString != "NULL")
            {
                join += "and tt.TestName in (" + testString + ") ";
            }
        }

        if ((organismId.Any() && !string.IsNullOrWhiteSpace(organismId.First().Value)) || includeCultureJoin)
        {
            var organismList = await CreateOrganismHierarchyAsync(queryFilters, connect);

            organismString = SqlSanitizer.SanitizeIdList(organismList);

            join += "inner join Culture c on c.SpecimenId = s.Id ";
            if (organismString != "NULL")
            {
                join += "and c.SpecimenOrganismId in (" + organismString + ") ";
            }

            if (includeAstJoin)
            {
                join += "inner join ast on ast.CultureId = c.Id ";

                if (susceptibilityId.Any() && !string.IsNullOrWhiteSpace(susceptibilityId.First().Value))
                {
                    var susString = SqlSanitizer.SanitizeIdList(string.Join(",", susceptibilityId.First().Value));
                    if (susString != "NULL")
                    {
                        join += "and ast.SusceptibilityId in (" + susString + ") ";
                    }
                }

                if (antibioticId.Any() && !string.IsNullOrWhiteSpace(antibioticId.First().Value))
                {
                    var antibioticString = SqlSanitizer.SanitizeIdList(string.Join(",", antibioticId.First().Value));
                    if (antibioticString != "NULL")
                    {
                        join += "and ast.AntibioticId in (" + antibioticString + ") ";
                    }
                }
            }
        }

        return join;
    }

    /// <summary>
    /// Builds a comma-separated list of tag IDs including all hierarchical descendants
    /// based on provided query filters and database connection.
    /// </summary>
    /// <param name="queryFilters">Filter config containing "tagid" parameter for IDs to expand.</param>
    /// <param name="connect">Open NpgsqlConnection for hierarchy queries.</param>
    /// <returns>
    /// Comma-separated string of unique tag IDs (including descendants), or empty if none supplied.
    /// </returns>
    public static async Task<string> CreateTagHierarchyAsync(QueryFilterConfig queryFilters, NpgsqlConnection connect)
    {
        var tagId = queryFilters.Parameters.Where(p => p.Key.Equals("tagid", StringComparison.OrdinalIgnoreCase));
        var tagList = "";
        if (tagId.Any() && !string.IsNullOrWhiteSpace(tagId.First().Value))
        {
            var tagHierarchy = new TagHierarchy();
            var allIds = new HashSet<int>();

            foreach (var part in tagId.First().Value.Split(","))
            {
                if (string.IsNullOrWhiteSpace(part) || !int.TryParse(part.Trim(), out var tagIdVal))
                {
                    continue;
                }

                var expanded = await tagHierarchy.GetStringList(connect, tagIdVal);
                if (!string.IsNullOrWhiteSpace(expanded))
                {
                    foreach (var idStr in expanded.Split(","))
                    {
                        if (int.TryParse(idStr.Trim(), out var id))
                        {
                            allIds.Add(id);
                        }
                    }
                }
            }

            tagList = allIds.Count > 0 ? string.Join(",", allIds) : "";
        }
        return tagList;
    }

    /// <summary>
    /// Builds a comma-separated list of organism IDs including all hierarchical descendants
    /// based on provided query filters and database connection.
    /// </summary>
    /// <param name="queryFilters">Filter config containing "organismid" parameter for IDs to expand.</param>
    /// <param name="connect">Open NpgsqlConnection for hierarchy queries.</param>
    /// <returns>
    /// Comma-separated string of unique organism IDs (including descendants), or empty if none supplied.
    /// </returns>
    public static async Task<string> CreateOrganismHierarchyAsync(QueryFilterConfig queryFilters, NpgsqlConnection connect)
    {
        var organismId = queryFilters.Parameters.Where(p => p.Key.Equals("organismid", StringComparison.OrdinalIgnoreCase));
        var organismList = "";
        if (organismId.Any() && !string.IsNullOrWhiteSpace(organismId.First().Value))
        {
            var organismQuery = new OrganismHierarchyQuery();
            var currentOrganismList = new List<int>();
            foreach (var organism in organismId.First().Value.Split(","))
            {
                var organismParam = new QueryFilterConfig { Parameters = [new() { Key = "id", Value = organism }] };
                currentOrganismList.AddRange(await organismQuery.ExecuteAsync(connect, organismParam));
            }
            organismList += string.Join(",", currentOrganismList.Distinct());
        }
        return organismList;
    }

    /// <summary>
    /// Appends a WHERE or AND clause filtering the specified field by the first record’s comma-separated values.
    /// </summary>
    /// <param name="idRecord">Collection of QueryValuesConfig whose Value is a comma-separated list of IDs.</param>
    /// <param name="whereClause">Current WHERE clause text to extend (empty or existing clause).</param>
    /// <param name="fieldName">Database field name (without alias) to filter on.</param>
    /// <param name="prefix">Optional table alias prefix (defaults to "s").</param>
    /// <returns>Updated WHERE clause including the new IN(...) filter, or original clause if no valid IDs.</returns>
    private static string AddWhereClause(IEnumerable<QueryValuesConfig> idRecord, string whereClause, string fieldName, string prefix = "s")
    {
        if (idRecord.Any() && !string.IsNullOrWhiteSpace(idRecord.First().Value))
        {
            var valueString = string.Join(",", idRecord.First().Value);
            whereClause += whereClause == "" ? "Where " : " and ";
            whereClause += prefix + "." + fieldName + " in (" + SqlSanitizer.SanitizeIdList(idRecord.First().Value) + ") ";
        }
        return whereClause;
    }

    /// <summary>
    /// Expands selected organizations into full hierarchy, enforces allowed organizations, and appends corresponding WHERE clause.
    /// </summary>
    /// <param name="selectedOrganisationId">Comma-separated list of organization IDs selected by the user.</param>
    /// <param name="allowedOrganisationId">Comma-separated list of organization IDs the user is permitted to view.</param>
    /// <param name="connect">Open NpgsqlConnection for hierarchy and permission queries.</param>
    /// <param name="whereClause">Existing WHERE clause text to extend.</param>
    /// <returns>Updated WHERE clause including organization filter, or original if no valid selections remain.</returns>
    private static async Task<string> GetCorrectOrganisationFilterAsync(string selectedOrganisationId, string allowedOrganisationId, NpgsqlConnection connect, string whereClause)
    {
        var organisationQuery = new OrganisationHierarchyListQuery();
        var selectedOrganisationList = "";
        var organisationList = selectedOrganisationId.Split(",");
        foreach (var organisation in organisationList)
        {
            if (organisation != null && !string.IsNullOrWhiteSpace(organisation))
            {
                var organisationParam = new QueryFilterConfig { Parameters = [new() { Key = "organisationid", Value = organisation }] };
                var tempOrganisationList = await organisationQuery.ExecuteAsync(connect, organisationParam);
                selectedOrganisationList += selectedOrganisationList == "" ? tempOrganisationList : "," + tempOrganisationList;
            }
        }

        if (allowedOrganisationId != null && !string.IsNullOrWhiteSpace(allowedOrganisationId) && !string.IsNullOrEmpty(selectedOrganisationId))
        {
            var orgParam = new QueryFilterConfig { Parameters = [new() { Key = "organisationid", Value = selectedOrganisationId }] };
            var allowedOrganisationList = await organisationQuery.ExecuteAsync(connect, orgParam);

            // Remove any organisations the user does not have permission to view

            var selectedArray = selectedOrganisationList.Split(",").ToList();
            var allowedArray = allowedOrganisationList.Split(",").ToList();
            selectedArray.RemoveAll(x => !allowedArray.Contains(x));
            selectedOrganisationList = string.Join(",", selectedArray);
        }

        if (selectedOrganisationList.Length > 0)
        {
            var orgList = string.Join(",", selectedOrganisationList);
            whereClause += whereClause == "" ? "Where " : " and ";
            whereClause += "s.OrganisationId in (" + SqlSanitizer.SanitizeIdList(orgList) + ") ";
        }

        return whereClause;
    }
}
