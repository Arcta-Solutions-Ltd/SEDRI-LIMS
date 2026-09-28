using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class QcOrganismMapperFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addiqctestprofilemapper" => new AddIqcTestProfileMapper(),
                "iqctestviewmapper" => new IqcTestViewMapper(),
                "qcorganismviewmapper" => new QcOrganismViewMapper(),
                _ => null,
            };
        }
    }
}
