using arc.app.Config;
using arc.app.SystemConfig;
using arc.common.Models.Config;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration
{
    public class ViewHandler : IViewHandler
    {
        private readonly IListViewConfigFactory _listViewFactory;
        private readonly IConfigRepository _configRepository;

        public ViewHandler(IListViewConfigFactory listViewFactory, IConfigRepository configRepository)
        {
            _listViewFactory = listViewFactory;
            _configRepository = configRepository;
        }

        public async Task<string> GetListView()
        {

            var parameters = new QueryFilterConfig { Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "ConfigTypeId", Value = "5" } } };
            var viewList = await _configRepository.GetConfigListAsync(parameters);
            var returnList = new List<ViewListModel>();

            foreach (var viewDetails in viewList)
            {
                var newItem = await _listViewFactory.GetViewAsync(viewDetails.ConfigName);
                var view = new ViewListModel { Name = newItem.Name, Title = newItem.Title, Id = viewDetails.Id };
                returnList.Add(view);
            }

            var list = returnList.OrderBy(o => o.Name).ToList();

            return JsonConvert.SerializeObject(list);
        }
    }
}
