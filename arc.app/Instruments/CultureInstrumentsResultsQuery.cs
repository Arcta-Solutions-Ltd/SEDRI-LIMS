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
    /// Represents a query to retrieve culture instrument results and run it asynchronously.
    /// </summary>
    internal class CultureInstrumentsResultsQuery : IQueryRun
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureInstrumentsResultsQuery"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider to resolve dependencies.</param>
        public CultureInstrumentsResultsQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Runs the query asynchronously to retrieve culture instrument results based on the specified query filters.
        /// </summary>
        /// <param name="queryFilter">The query filter configuration.</param>
        /// <param name="token">The token information model.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the serialized list of instrument results.</returns>
        public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
        {
            var instrumentRepository = _serviceProvider.GetRequiredService<IInstrumentRepository>();
            var instrumentList = await instrumentRepository.GetCultureInstrumentResultsAsync(queryFilter);

            return JsonConvert.SerializeObject(instrumentList);
        }
    }

}
