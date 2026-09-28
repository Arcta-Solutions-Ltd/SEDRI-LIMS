using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class ListMapperFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addtableentrymapper" => new AddTableEntryMapper(),
                "addtablemapper" => new AddTableMapper(),
                "deletetablemapper" => new DeleteTableMapper(),
                "edittablemapper" => new EditTableMapper(),
                "edittablequerymapper" => new EditTableQueryMapper(),
                "checkwhetherlistitemisfixedmapper" => new CheckWhetherListItemIsFixedMapper(),
                "checkwhethertableentryisfixedmapper" => new CheckWhetherTableEntryIsFixedMapper(),
                _ => null,
            };
        }
    }
}
