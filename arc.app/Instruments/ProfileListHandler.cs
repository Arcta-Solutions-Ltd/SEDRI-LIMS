using arc.common.Models;
using arc.common.Models.Instruments;
using arc.domain.Configuration.ListsConfig;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Instruments
{
    public class ProfileListHandler : IProfileListHandler
    {
        private readonly IServiceProvider _serviceProvider;

        public ProfileListHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task<List<OptionsConfig>> GetListAsync(TokenInfoModel token)
        {
            var listHandler = new InstrumentListQuery(_serviceProvider);
            var profileListAsString = await listHandler.RunAsync(new domain.Configuration.QueryFiltersConfig.QueryFilterConfig(), token);
            var profileList = JsonConvert.DeserializeObject<List<InstrumentProfileListModel>>(profileListAsString);
            return profileList.Select(p => new OptionsConfig { Key = p.InstrumentName, Text = p.InstrumentName }).ToList();
        }
    }
}
