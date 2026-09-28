using arc.common.ExtensionMethods;
using arc.common.Models.Tests;
using arc.data.Extensions;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arc.data.Tests;

/// <summary>
/// Query to retrieve active test list with SQL injection protection.
/// All user input is sanitized before being used in SQL queries.
/// </summary>
internal class ActiveTestListQuery : IQueryReturningType<List<TestListResultModel>>
{
    private const int _requestedTestStatus = 1195;

    public async Task<List<TestListResultModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilter)
    {
        var whereClause = new StringBuilder($"where {QuerySecurity.GetWhereClause(queryFilter)}");

        var queryFilterHasTestStatusId = queryFilter.TryParseIntegerValue("teststatus", out var testStatusId);
        var queryFilterHasTestTypeCsv = queryFilter.TryGetStringValue("testtype", out var testTypeCsv);
        var queryFilterHasAllowedStates = queryFilter.TryGetStringValue("allowedstates", out var allowedStatesCsv);
        var queryFilterHasSpecimenTypeId = queryFilter.TryGetStringValue("specimentypeid", out var specimenTypeIdCsv);
        var queryFilterHasCultureTypeId = queryFilter.TryGetStringValue("culturetypeid", out var cultureTypeIdCsv);
        var queryFilterHasPatientRef = queryFilter.TryGetStringValue("patientref", out var patientRef);
        var queryFilterHasType = queryFilter.TryGetStringValue("type", out var type);

        if (queryFilterHasTestStatusId)
        {
            // testStatusValue is derived from integer comparison, not user input - safe
            var testStatusValue = testStatusId == _requestedTestStatus ? "Requested" : "Complete";
            whereClause.AppendLine($"and t.status = '{testStatusValue}' ");
        }

        if (queryFilterHasTestTypeCsv)
        {
            // Sanitize test type CSV to prevent SQL injection
            var sanitizedTestTypes = SqlSanitizer.SanitizeStringList(testTypeCsv);
            whereClause.AppendLine($"and t.testname in ({sanitizedTestTypes}) ");
        }

        if (queryFilterHasAllowedStates)
        {
            // Sanitize ID list to only allow valid integers
            var sanitizedStates = SqlSanitizer.SanitizeIdList(allowedStatesCsv);
            whereClause.AppendLine($"and s.stateid in ({sanitizedStates})");
        }

        if (queryFilterHasSpecimenTypeId)
        {
            // Sanitize ID list to only allow valid integers
            var sanitizedSpecimenTypes = SqlSanitizer.SanitizeIdList(specimenTypeIdCsv);
            whereClause.AppendLine($"and s.SpecimenTypeId in ({sanitizedSpecimenTypes})");
        }

        if (queryFilterHasCultureTypeId)
        {
            // Sanitize ID list to only allow valid integers
            var sanitizedCultureTypes = SqlSanitizer.SanitizeIdList(cultureTypeIdCsv);
            whereClause.AppendLine($"and c.TypeId in ({sanitizedCultureTypes})");
        }

        // Search filters - all user input must be sanitized
        if (queryFilterHasPatientRef)
        {
            queryFilter.TryGetStringValue("firstname", out var firstName);
            queryFilter.TryGetStringValue("surname", out var surname);
            queryFilter.TryGetStringValue("accessionnumber", out var accessionNumber);
            queryFilter.TryGetStringValue("barcode", out var barcode);
            queryFilter.TryGetStringValue("existingbarcode", out var existingBarcode);
            queryFilter.TryGetStringValue("status", out var status);
            queryFilter.TryGetStringValue("specimentype", out var specimenType);

            // Sanitize all string inputs to prevent SQL injection
            var sanitizedPatientRef = SqlSanitizer.SanitizeSearchText(patientRef?.Trim() ?? "");
            var sanitizedFirstName = SqlSanitizer.SanitizeSearchText(firstName?.Trim() ?? "");
            var sanitizedSurname = SqlSanitizer.SanitizeSearchText(surname?.Trim() ?? "");
            var sanitizedAccessionNumber = SqlSanitizer.SanitizeSearchText(accessionNumber?.Trim() ?? "");
            var sanitizedBarcode = SqlSanitizer.SanitizeValue(barcode?.Trim() ?? "");
            var sanitizedExistingBarcode = SqlSanitizer.SanitizeValue(existingBarcode?.Trim() ?? "");
            var sanitizedStatus = SqlSanitizer.SanitizeSearchText(status?.Trim() ?? "");
            var sanitizedSpecimenType = SqlSanitizer.SanitizeSearchText(specimenType?.Trim() ?? "");

            whereClause.AppendLine($"""
                and (
                    p.PatientRef ilike '{sanitizedPatientRef}%'
                    or p.FirstName ilike '{sanitizedFirstName}%'
                    or p.Surname ilike '{sanitizedSurname}%'
                    or s.AccessionNumber ilike '%{sanitizedAccessionNumber}%'
                    or s.Barcode = '{sanitizedBarcode}'
                    or s.ExistingBarcode = '{sanitizedExistingBarcode}'
                    or t.status ilike '{sanitizedStatus}%'
                    or li.value ilike '{sanitizedSpecimenType}%'
                """);

            if (queryFilterHasType && type != "direct")
            {
                queryFilter.TryGetStringValue("culturetype", out var cultureType);
                var sanitizedCultureType = SqlSanitizer.SanitizeSearchText(cultureType?.Trim() ?? "");
                whereClause.AppendLine($"""
                        or li2.value ilike '{sanitizedCultureType}%'
                    """);
            }

            whereClause.AppendLine($"""
                    )
                    """);
        }

        var orderByDictionary = new Dictionary<string, string>
        {
            { "accessionnumber", "s.accessionnumber" },
            { "testdescription", "t.testname" },
            { "status", "t.status" },
            { "requested", "t.requested" },
            { "completed", "t.completed" },
            { "firstname", "p.firstname" },
            { "surname", "p.surname" },
            { "patientref", "p.patientref" },
            { "specimentype", "li.value" },
            { "culturetype", "li2.value" }
        };
        var orderBy = orderByDictionary[queryFilter.OrderBy.ToLower()];

        orderBy += queryFilter.OrderDescending ? "" : " desc";

        string sql;
        if (queryFilterHasType && type == "direct")
        {
            sql = $"""
                select
                    t.id,
                    t.testname,
                    t.status,
                    t.Requested AT TIME ZONE 'UTC' As Requested,
                    t.completed AT TIME ZONE 'UTC' As completed,
                    t.testresults,
                    al.colour,
                    al.alertcategoryid,
                    s.accessionnumber,
                    s.id as specimenid,
                    s.laboratoryid,
                    p.firstname,
                    p.surname,
                    p.patientref,
                    li.value as specimentype
                from
                    Tests t
                    inner join specimen s on s.id = t.specimenid
                    inner join patient p on p.id = s.patientid
                    inner join listitem li on li.id = s.specimentypeid
                    left outer join alerttype al on al.id = t.alerttypeid
                {whereClause}
                order by {orderBy}
                """;
        }
        else
        {
            sql = $"""
                select
                    t.id,
                    t.testname,
                    t.status,
                    t.Requested AT TIME ZONE 'UTC' As Requested,
                    t.completed AT TIME ZONE 'UTC' As completed,
                    t.testresults,
                    al.colour,
                    al.alertcategoryid,
                    s.accessionnumber,
                    s.id as specimenid,
                    s.laboratoryid,
                    p.firstname,
                    p.surname,
                    p.patientref,
                    li.value as specimentype,
                    li2.value as culturetype
                from
                    CultureTests t
                    inner join culture c on c.id = t.cultureid
                    inner join specimen s on s.id = c.specimenid
                    inner join patient p on p.id = s.patientid
                    inner join listitem li on li.id = s.specimentypeid
                    left outer join listitem li2 on li2.id = c.typeid
                    left outer join alerttype al on al.id = t.alerttypeid
                {whereClause}
                order by {orderBy}
                """;
        }

        var result = await connect.QueryAsync<TestListResultModel>(sql);

        return result.ToList();
    }
}
