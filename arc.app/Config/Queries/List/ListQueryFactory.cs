using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class ListQueryFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "checkwhetherlistitemisfixed" => new CheckWhetherListItemIsFixedQuery(),
                "checkwhethertableentryisfixed" => new CheckWhetherTableEntryIsFixedQuery(),
                "duplicatelistnamequery" => new DuplicateListNameQuery(),
                "duplicatelistitemquery" => new DuplicateListItemQuery(),
                "edittablequery" => new EditTableQuery(),
                "itemsinlistcountquery" => new ItemsInListCountQuery(),
                "listbyid" => new ListByIdQuery(),
                "listcontents" => new ListContentsQuery(),
                "listentrybyidforedit" => new ListEntryByIdForEditQuery(),
                "ordertablequery" => new OrderTableQuery(),
                "singletableentryforlist" => new SingleTableEntryForListQuery(),
                "updatetablelistquery" => new UpdateTableListQuery(),
                _ => null,
            };
        }
    }
}
