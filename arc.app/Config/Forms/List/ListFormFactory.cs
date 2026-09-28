using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class ListFormFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addtableform" => new AddTableFormConfig(),
                "addtableentryform" => new AddTableEntryFormConfig(),
                "edittableform" => new EditTableFormConfig(),
                "edittableentryform" => new EditTableEntryFormConfig(),
                "deletetableform" => new DeleteTableFormConfig(),
                "deletetableentryform" => new DeleteTableEntryFormConfig(),
                "ordertableform" => new OrderTableFormConfig(),
                _ => null,
            };
        }
    }
}
