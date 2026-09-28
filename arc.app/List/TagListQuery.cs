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
    /// Executes the TagList query for the tags list view.
    /// Returns all tags (ListItem where ListId=105) as JSON.
    /// </summary>
    internal class TagListQuery : IQueryRun
    {
        private readonly IServiceProvider _serviceProvider;

        public TagListQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Runs the tag list query and returns the result as JSON.
        /// </summary>
        /// <param name="queryFilter">Filter configuration (supports searchText).</param>
        /// <param name="token">User token for authorization.</param>
        /// <returns>JSON-serialized list of tags.</returns>
        public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
        {
            var listRepository = _serviceProvider.GetService<IListRepository>();
            var tagList = await listRepository.GetTagListForViewAsync(queryFilter);
            return JsonConvert.SerializeObject(tagList);
        }
    }
}
