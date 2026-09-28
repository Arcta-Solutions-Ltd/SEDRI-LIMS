using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Quality
{
    internal class MarkIqcTestCompleteEvent : IRun
    {
        private readonly IQualityRepository _qualityRepository;
        private readonly IListRepository _listRepository;

        public MarkIqcTestCompleteEvent(IServiceProvider serviceProvider)
        {
            _qualityRepository = serviceProvider.GetService<IQualityRepository>();
            _listRepository = serviceProvider.GetService<IListRepository>();
        }

        public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
        {
            var queryFilterConfig = new QueryFilterConfig();
            queryFilterConfig.AddString("value", "Completed");
            queryFilterConfig.AddString("name", "IqcTestState");
            var testMethodListItem = await _listRepository.GetListItemByValueAsync(queryFilterConfig);
            var id = int.Parse(Id);

            await _qualityRepository.MarkIqcTestCompleteAsync(id, testMethodListItem.Id);
            return id;
        }
    }
}
