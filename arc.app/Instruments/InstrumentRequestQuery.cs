using arc.app.Common;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace arc.app.Instruments
{
    internal class InstrumentRequestQuery : IQueryRun
    {
        private readonly IServiceProvider _serviceProvider;

        internal InstrumentRequestQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
        {
            var instrumentRepository = _serviceProvider.GetService<IInstrumentRepository>();

            var requestList = await instrumentRepository.GetNextRequestAsync(queryFilter);

            return JsonConvert.SerializeObject(requestList);
        }
    }
}
