using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class OrganisationMapperFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "organisationchildcountmapper" => new OrganisationChildCountMapper(),
                "organisationspecimencountmapper" => new OrganisationSpecimenCountMapper(),
                "organisationusercountmapper" => new OrganisationUserCountMapper(),
                _ => null,
            };
        }
    }
}
