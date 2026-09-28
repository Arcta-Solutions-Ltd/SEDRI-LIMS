using arc.app.Config.List.Specimen;
using arc.domain.Configuration.ListsConfig;

namespace arc.app.Config
{
    public class ListConfigFactory : IListConfigFactory
    {
        public ListConfig GetList(string listName)
        {
            return listName.ToLower() switch
            {
                "receiveddate" => new ReceivedDateListConfig().GetList(),
                _ => null
            };
        }
    }
}
