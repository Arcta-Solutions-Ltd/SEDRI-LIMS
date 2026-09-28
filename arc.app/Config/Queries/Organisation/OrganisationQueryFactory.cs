using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class OrganisationQueryFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "organisationbyid" => new OrganisationByIdQuery(),
                "organisationchildcount" => new OrganisationChildCountQuery(),
                "organisationforvalidation" => new OrganisationForValidationQuery(),
                "organisationlist" => new OrganisationListQuery(),
                "organisationspecimencount" => new OrganisationSpecimenCountQuery(),
                "organisationusercount" => new OrganisationUserCountQuery(),
                "singleorganisationfororganisationlist" => new SingleOrganisationForOrganisationListQuery(),
                _ => null,
            };
        }
    }
}
