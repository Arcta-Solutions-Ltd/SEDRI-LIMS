using arc.app.Common;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace arc.app.List
{
    /// <summary>
    /// Executes the SingleTagForTagList query for edit/delete forms.
    /// Returns a single tag by Id as JSON.
    /// </summary>
    internal class SingleTagForTagListQuery : IQueryRun
    {
        private readonly IServiceProvider _serviceProvider;

        public SingleTagForTagListQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Runs the single tag query and returns the result as JSON.
        /// </summary>
        /// <param name="queryFilter">Filter configuration; expects 'id' parameter.</param>
        /// <param name="token">User token for authorization.</param>
        /// <returns>JSON-serialized tag or empty object if not found.</returns>
        public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
        {
            var listRepository = _serviceProvider.GetService<IListRepository>();
            var tag = await listRepository.GetSingleTagForTagListAsync(queryFilter);
            if (tag == null)
            {
                return JsonConvert.SerializeObject(new { Id = 0, TagId = 0, Name = "", ParentTagId = (int?)null, Enabled = "Yes" });
            }
            return JsonConvert.SerializeObject(new { Id = tag.Id, TagId = tag.Id, Name = tag.Value, ParentTagId = tag.ParentTagId, Enabled = tag.Enabled ? "Yes" : "No" });
        }
    }
}
