using arc.app.Common;
using arc.app.SystemConfig;
using arc.common.Models.Config;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration.Queries
{
    /// <summary>
    /// Retrieves parent-link metadata for add field configuration (child lists, form list fields).
    /// </summary>
    internal class GetFieldParentLinkQuery : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFieldParentLinkQuery"/> class.
        /// </summary>
        /// <param name="serviceProvider">Service provider for dependency resolution.</param>
        internal GetFieldParentLinkQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Returns child list hierarchy and list-backed fields on the target form.
        /// </summary>
        /// <param name="queryFilters">Query filters containing id as formName|pageName or formName|pageName|fieldId.</param>
        /// <param name="queryData">Query definition (unused).</param>
        /// <returns>JSON serialized <see cref="FieldParentLinkQueryResultModel"/>.</returns>
        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var idParam = queryFilters.Parameters?.FirstOrDefault(p => p.Key?.ToLower() == "id");
            var id = idParam?.Value ?? "";
            var idList = id.Split("|");
            var formName = idList.Length > 0 ? idList[0] : "";
            var excludeFieldId = idList.Length > 2 ? idList[2] : null;

            var logWriter = _serviceProvider.GetService<ILogWriter>();
            logWriter?.LogInfo(
                $"GetFieldParentLinkQuery: formName={formName}, excludeFieldId={excludeFieldId ?? "(none)"}",
                nameof(GetFieldParentLinkQuery),
                nameof(GetAsync));

            var formConfigDefinition = _serviceProvider.GetService<IFormConfigDefinition>();
            var listRepository = _serviceProvider.GetService<IListRepository>();
            var utils = new FieldParentLinkUtils(formConfigDefinition, listRepository, logWriter);
            var result = await utils.BuildAsync(formName, excludeFieldId);

            logWriter?.LogInfo(
                $"GetFieldParentLinkQuery: childLists={result.ChildLists?.Count ?? 0}, pageListFields={result.PageListFields?.Count ?? 0}",
                nameof(GetFieldParentLinkQuery),
                nameof(GetAsync));

            return JsonConvert.SerializeObject(result);
        }
    }
}
