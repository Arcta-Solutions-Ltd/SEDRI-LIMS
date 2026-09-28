using arc.app.Common;
using arc.common.ExtensionMethods;
using arc.common.Models.Organisation;
using arc.common.Utils;
using arc.data.Utils;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Organisation;

internal class EditOrganisationCommand : ICommandWithTypeReturningInteger<OrganisationModel>
{
    private readonly IJsonReplacer _jsonReplacer;

    public EditOrganisationCommand(IJsonReplacer jsonReplacer)
    {
        _jsonReplacer = jsonReplacer;
    }

    public async Task<int> ExecuteAsync(NpgsqlConnection connect, OrganisationModel organisation, ILogWriter logWriter)
    {
        var targetId = int.Parse(organisation.Id);

        var codeHierarchy = "";
        if (!string.IsNullOrEmpty(organisation.ParentOrganisationId))
        {
            var parentSql = @"select id, organisationname, fullyqualifiedname, parentorganisationid, moredata ->> 'organisationcodehierarchy' as organisationcodehierarchy from organisation where id = @Id";
            var parentOrg = await connect.QueryFirstAsync<OrganisationModel>(parentSql, new { Id = int.Parse(organisation.ParentOrganisationId) });
            organisation.FullyQualifiedName = parentOrg.FullyQualifiedName + " : " + organisation.OrganisationName;
            codeHierarchy = $"{parentOrg.OrganisationCodeHierarchy} : {organisation.Code}";
            organisation.OrganisationCodeHierarchy = $"{parentOrg.OrganisationCodeHierarchy} : {organisation.Code}";
        }
        else
        {
            organisation.FullyQualifiedName = organisation.OrganisationName;
            organisation.OrganisationCodeHierarchy = organisation.Code;
            codeHierarchy = $"{organisation.Code}";
        }

        organisation.MoreData = _jsonReplacer.AddNewStringValue(organisation.MoreData, "organisationcodehierarchy", codeHierarchy);

        var sql = @"update organisation set organisationname = @OrganisationName, code = @Code, fullyqualifiedname = @FullyQualifiedName, enabled = @Enabled, locationid = @LocationId,
                        parentorganisationid = @ParentOrganisationId, languageid = @LanguageId, lastmodifieddate = now(), moredata = cast(@MoreData As json) Where  Id = @Id";

        await connect.ExecuteAsync(sql, new
        {
            Id = targetId,
            organisation.OrganisationName,
            organisation.FullyQualifiedName,
            ParentOrganisationId = !string.IsNullOrEmpty(organisation.ParentOrganisationId) ? int.Parse(organisation.ParentOrganisationId) : (int?)null,
            LanguageId = int.Parse(organisation.LanguageId),
            organisation.MoreData,
            organisation.Enabled,
            organisation.LocationId,
            organisation.Code
        });

        // Find and update the fully-qualified name of all dependent organisations.
        var orgIds = await new OrganisationIdListFromOrganisationIdQuery().ExecuteAsync(connect, targetId);
        var organisationString = string.Join(",", orgIds);
        var sanitizedOrgIds = SqlSanitizer.SanitizeIdList(organisationString);
        var whereClause = " where Id in (" + sanitizedOrgIds + ") ";
        sql = @"select id, organisationname, fullyqualifiedname, parentorganisationid, moredata ->> 'organisationcodehierarchy' as organisationcodehierarchy, code from organisation" + whereClause;
        var result = await connect.QueryAsync<OrganisationModel>(sql);
        var orgList = result.ToList();

        foreach (var org in orgList)
        {
            if (org.Id == organisation.Id) { continue; }
            var orgWalker = org;
            var fullyQualifiedName = orgWalker.OrganisationName;
            var organisationCodeHierarchy = orgWalker.Code;
            while (orgWalker.ParentOrganisationId != organisation.Id)
            {
                orgWalker = orgList.Where(o => o.Id == orgWalker.ParentOrganisationId).First();
                fullyQualifiedName = orgWalker.OrganisationName + " : " + fullyQualifiedName;
                organisationCodeHierarchy = orgWalker.Code + " : " + organisationCodeHierarchy;
            }
            fullyQualifiedName = organisation.FullyQualifiedName + " : " + fullyQualifiedName;
            organisationCodeHierarchy = organisation.OrganisationCodeHierarchy + " : " + organisationCodeHierarchy;
            var moreData = $"{{\r\n  \"organisationcodehierarchy\": \"{organisationCodeHierarchy}\"\r\n}}";

            sql = @"update organisation set fullyqualifiedname = @FullyQualifiedName, lastmodifieddate = now(), moredata = cast(@MoreData As json) Where  Id = @Id";

            await connect.ExecuteAsync(sql, new
            {
                Id = int.Parse(org.Id),
                fullyQualifiedName,
                moreData
            });
        }

        return targetId;
    }
}
