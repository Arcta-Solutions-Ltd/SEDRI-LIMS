using arc.app.Common;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace arc.app.Instruments
{
    /// <summary>
    /// Represents a query to retrieve instrument results list and run it asynchronously.
    /// </summary>
    internal class InstrumentResultsListQuery : IQueryRun
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstrumentResultsListQuery"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider to resolve dependencies.</param>
        public InstrumentResultsListQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <summary>
        /// Runs the query asynchronously to retrieve instrument results list based on the specified query filter.
        /// </summary>
        /// <param name="queryFilter">The query filter configuration.</param>
        /// <param name="token">The token information model.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the serialized list of instrument results.</returns>
        public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
        {
            if (queryFilter == null)
            {
                throw new ArgumentNullException(nameof(queryFilter));
            }

            var instrumentRepository = _serviceProvider.GetRequiredService<IInstrumentRepository>();
            var instrumentList = await instrumentRepository.GetInstrumentResultsListAsync(queryFilter);

            return JsonConvert.SerializeObject(instrumentList);
        }
    }
}
