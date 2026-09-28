using arc.app.Common;
using arc.common.Models.Organisation;
using arc.common.Utils;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Organisation
{
    internal class AddOrganisationCommand : ICommandWithTypeReturningInteger<OrganisationModel>
    {
        private readonly IJsonReplacer _jsonReplacer;

        public AddOrganisationCommand(IJsonReplacer jsonReplacer) {

            _jsonReplacer = jsonReplacer;

        }

        public async Task<int> ExecuteAsync(NpgsqlConnection connect, OrganisationModel organisation, ILogWriter logWriter)
        {
            var codeHierarchy = "";
            if (!string.IsNullOrEmpty(organisation.ParentOrganisationId))
            {
                var parentSql = @"select id, organisationname, fullyqualifiedname, parentorganisationid, moredata ->> 'organisationcodehierarchy' as organisationcodehierarchy from organisation where id = @Id";
                var parentOrg = await connect.QueryFirstAsync<OrganisationModel>(parentSql, new { Id = int.Parse(organisation.ParentOrganisationId) });
                organisation.FullyQualifiedName = parentOrg.FullyQualifiedName + " : " + organisation.OrganisationName;
                codeHierarchy = $"{organisation.OrganisationCodeHierarchy} : {organisation.Code}";
            }
            else
            {
                organisation.FullyQualifiedName = organisation.OrganisationName;
                codeHierarchy = $"{organisation.Code}";
            }

            organisation.MoreData = _jsonReplacer.AddNewStringValue(organisation.MoreData, "organisationcodehierarchy", codeHierarchy);

            var sql = @"insert into organisation(organisationname, code, fullyqualifiedname, parentorganisationid, languageid, lastmodifieddate, moredata, enabled, locationid)
                        values(@OrganisationName, @Code, @FullyQualifiedName, @ParentOrganisationId, @LanguageId, now(), cast(@MoreData as json), @Enabled, @LocationId) returning id";

            return await connect.ExecuteAsync(sql, new
            {
                organisation.OrganisationName,
                organisation.FullyQualifiedName,
                ParentOrganisationId = !string.IsNullOrEmpty(organisation.ParentOrganisationId) ? int.Parse(organisation.ParentOrganisationId) : (int?)null,
                LanguageId = int.Parse(organisation.LanguageId),
                organisation.MoreData,
                organisation.Enabled,
                organisation.LocationId,
                organisation.Code
            });
        }
    }
}
