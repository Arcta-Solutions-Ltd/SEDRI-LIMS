using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class ListUIEventFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addtableuievent" => new AddTableUIEventConfig(),
                "addtableentryuievent" => new AddTableEntryUIEventConfig(),
                "deletetableuievent" => new DeleteTableUIEventConfig(),
                "deletetableentryuievent" => new DeleteTableEntryUIEventConfig(),
                "edittableuievent" => new EditTableUIEventConfig(),
                "edittableentryuievent" => new EditTableEntryUIEventConfig(),
                "ordertableuievent" => new OrderTableUIEventConfig(),
                _ => null,
            };
        }
    }
}
