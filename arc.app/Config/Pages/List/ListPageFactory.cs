using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class ListPageFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addtablepage" => new AddTablePageConfig(),
                "deletetablepage" => new DeleteTablePageConfig(),
                "edittablepage" => new EditTablePageConfig(),   
                "edittableentrypage" => new EditTableEntryPageConfig(),
                "deletetableentrypage" => new DeleteTableEntryPageConfig(),
                "ordertablepage" => new OrderTablePageConfig(),
                "tableentrypage" => new TableEntryPageConfig(),
                _ => null,
            };
        }
    }
}
