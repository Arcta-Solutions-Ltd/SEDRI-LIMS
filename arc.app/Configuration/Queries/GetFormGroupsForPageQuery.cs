using arc.app.Common;
using arc.domain.Configuration.PagesConfig;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration.Queries
{
    /// <summary>
    /// Retrieves the list of form groups for a page, aggregated across all columns.
    /// Used for add form group dropdown/selector and form definition display.
    /// </summary>
    internal class GetFormGroupsForPageQuery : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        internal GetFormGroupsForPageQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Returns form groups as JSON array. Id parameter: formName|pageName.
        /// </summary>
        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var idParam = queryFilters.Parameters?.FirstOrDefault(p => p.Key?.ToLower() == "id");
            var id = idParam?.Value ?? "";
            var idList = id.Split("|");
            if (idList.Length < 2)
            {
                return JsonConvert.SerializeObject(new List<object>());
            }

            var formName = idList[0];
            var pageName = idList[1];

            var logWriter = _serviceProvider.GetService<ILogWriter>();
            logWriter?.LogInfo($"GetFormGroupsForPageQuery: formName={formName}, pageName={pageName}", "GetFormGroupsForPageQuery", "GetAsync");

            var formConfigDefinition = _serviceProvider.GetService<IFormConfigDefinition>();
            var form = await formConfigDefinition.LoadFormAsync(formName);
            var page = form?.PagesConfig?.FirstOrDefault(p => p.Name?.Equals(pageName, StringComparison.OrdinalIgnoreCase) == true);

            var selectorValues = new List<object>();
            var ordinal = 1;
            if (page?.Columns != null)
            {
                foreach (var col in page.Columns)
                {
                    foreach (var fg in col.FormGroups ?? new List<FormGroupConfig>())
                    {
                        var value = $"{col.Key}|{fg.Key}";
                        var label = $"Subsection {ordinal}";
                        selectorValues.Add(new { id = label, label, value });
                        ordinal++;
                    }
                }
            }

            var result = new { Id = $"{formName}|{pageName}", FieldList = selectorValues };
            return JsonConvert.SerializeObject(result);
        }
    }
}
