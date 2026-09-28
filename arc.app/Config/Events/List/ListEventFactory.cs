using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class ListEventFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {

            return definitionName.ToLower() switch
            {
                "addtable" => new AddTableEventConfig(),
                "addtableentry" => new AddTableEntryEventConfig(),
                "edittable" => new EditTableEventConfig(),
                "edittableentry" => new EditTableEntryEventConfig(),
                "deletetable" => new DeleteTableEventConfig(),
                "deletetableentry" => new DeleteTableEntryEventConfig(),
                "ordertable" => new OrderTableEventConfig(),
                _ => null,
            };
        }
    }
}
