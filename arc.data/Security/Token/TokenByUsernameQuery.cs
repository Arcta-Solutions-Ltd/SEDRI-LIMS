using arc.common.ExtensionMethods;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arc.data.Security.Token;

/// <summary>
/// Represents a query for retrieving token information based on a username.
/// </summary>
internal class TokenByUsernameQuery : IQueryReturningType<TokenInfoModel>
{
    /// <summary>
    /// Executes the query to retrieve token information for a specified username.
    /// </summary>
    /// <param name="connect">The database connection to execute the query.</param>
    /// /// <param name="queryFilters">Contains filtering parameters for the query.</param>
    /// <returns>A Task resolving to a TokenInfoModel with token details.</returns>
    public async Task<TokenInfoModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var username = queryFilters.GetStringValue("username");
        var labid = queryFilters.GetStringValue("labid");
        var orgid = queryFilters.GetStringValue("orgid");
        var emailAddress = queryFilters.GetStringValue("emailaddress");

        var sql = new StringBuilder("""
            SELECT
                s.Id,
                s.Username,
                s.FirstName,
                s.LastName,
                s.Enabled,
                (
                    SELECT
                        STRING_AGG (cast(l.id as varchar(10)), ', ')
                    FROM
                        laboratoryuser lu
                        JOIN laboratory l ON l.id = lu.laboratoryid
                    WHERE
                        lu.userid = s.id
                ) AS AllowedLaboratories,
                (
                    SELECT
                        STRING_AGG (cast(o.id as varchar(10)), ', ')
                    FROM
                        organisationuser ou
                        JOIN organisation o ON o.id = ou.organisationid
                    WHERE
                        ou.userid = s.id
                        AND o.enabled = 'Yes'
                ) AS AllowedOrganisations
            FROM
                users s
            WHERE
                s.username = @username
            """);

        if (string.IsNullOrEmpty(username))
        {
            sql.Append($"""
                {Environment.NewLine}
                or
                   s.email = @emailAddress
                """);
        }

        var result = await connect.QueryAsync<TokenInfoModel>(sql.ToString(), new { username, emailAddress });
        var userRecord = result.First();
        bool identifiedLabOrOrgId = false;

        if (labid.IsIntegerGreaterThan(0) && userRecord.AllowedLaboratories.CsvContains(labid))
        {
            userRecord.LaboratoryId = labid;
            userRecord.OrganisationId = "0";
            identifiedLabOrOrgId = true;
        }

        if (orgid.IsIntegerGreaterThan(0) && userRecord.AllowedOrganisations.CsvContains(orgid))
        {
            userRecord.OrganisationId = orgid;
            userRecord.LaboratoryId = "0";
            identifiedLabOrOrgId = true;
        }

        if (!identifiedLabOrOrgId)
        {
            userRecord.LaboratoryId = userRecord.AllowedLaboratories?.Split(',').FirstOrDefault() ?? "0";
            userRecord.OrganisationId = userRecord.LaboratoryId == "0"
                ? userRecord.AllowedOrganisations?.Split(',').FirstOrDefault() ?? "0"
                : "0";
        }

        userRecord.LanguageId = await StaticTokenUtils.GetLanguageIdFormLabIdOrOrganisationIdAsync(connect, userRecord.LaboratoryId, userRecord.OrganisationId);
        return userRecord;
    }
}


