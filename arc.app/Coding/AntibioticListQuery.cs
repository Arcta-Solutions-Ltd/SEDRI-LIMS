using arc.app.Common;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace arc.app.Coding
{
    internal class AntibioticListQuery : IQueryRun
    {
        private readonly IServiceProvider _serviceProvider;

        public AntibioticListQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
        {
            var antibioticRepository = _serviceProvider.GetService<IAntibioticRepository>();

            var antibioticList = await antibioticRepository.GetAntibioticListForViewAsync(queryFilter);

            return JsonConvert.SerializeObject(antibioticList);
        }
    }
}
