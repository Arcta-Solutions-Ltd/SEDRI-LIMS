using arc.app.Common;
using arc.common;
using arc.common.Models.Quality;
using arc.common.Models.Role;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Quality
{
    internal class AddIqcTestEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public AddIqcTestEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
        {
            var data = JsonConvert.DeserializeObject<dynamic>(dataToSave);
            JArray toggles = data.qcorganisms;
            var ids = toggles
                .Select(x => x.ToObject<CraftedSelectionsModel>())
                .Where(x => x.Allowed == "Yes")
                .Select(x => int.Parse(x.Key))
                .ToList();

            AddIqcTestModel addIqcTestModel = new AddIqcTestModel()
            {
                TestProfileId = data.TestProfileId,
                IqcTestProfileQcOrganismIds = ids
            };

            return await _serviceProvider.GetService<IQualityRepository>().AddIqcTestAsync(addIqcTestModel);
        }
    }
}
