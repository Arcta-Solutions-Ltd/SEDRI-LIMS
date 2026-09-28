using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class QueueQueryFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "itemscontentsquery" => new ItemsContentsQuery(),
                "queuelist" => new QueueListQuery(),
                "queueitemjsoncontents" => new QueueItemJsonContentsQuery(),
                _ => null,
            };
        }
    }
}
