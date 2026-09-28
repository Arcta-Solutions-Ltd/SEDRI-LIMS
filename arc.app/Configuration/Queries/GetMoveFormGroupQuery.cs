using arc.app.Common;
using arc.domain.Configuration.FormStructureConfig;
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
    /// Retrieves data for the Move Subsection form.
    /// Id parameter: formName|pageName|columnKey|formGroupKey.
    /// Returns pageOptions for all pages except the source page.
    /// </summary>
    internal class GetMoveFormGroupQuery : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        internal GetMoveFormGroupQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Returns JSON with Id, SubsectionLabel, TargetPageId, and pageOptions (excluding source page).
        /// </summary>
        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var idParam = queryFilters.Parameters?.FirstOrDefault(p => p.Key?.ToLower() == "id");
            var id = idParam?.Value ?? "";
            var idList = id.Split("|");
            if (idList.Length < 4)
            {
                return JsonConvert.SerializeObject(new { Id = id, SubsectionLabel = "", TargetPageId = "", pageOptions = new List<object>() });
            }

            var formName = idList[0];
            var sourcePageName = idList[1];
            var columnKey = idList[2];
            var formGroupKey = idList[3];

            var logWriter = _serviceProvider.GetService<ILogWriter>();
            logWriter?.LogInfo(
                $"GetMoveFormGroupQuery: formName={formName}, sourcePage={sourcePageName}, formGroupKey={formGroupKey}",
                "GetMoveFormGroupQuery",
                "GetAsync");

            var formConfigDefinition = _serviceProvider.GetService<IFormConfigDefinition>();
            var form = await formConfigDefinition.LoadFormAsync(formName);
            if (form == null)
            {
                return JsonConvert.SerializeObject(new { Id = id, SubsectionLabel = formGroupKey, TargetPageId = "", pageOptions = new List<object>() });
            }

            var sourcePage = form.PagesConfig?.FirstOrDefault(p => p.Name?.Equals(sourcePageName, StringComparison.OrdinalIgnoreCase) == true);
            var subsectionOrdinal = 0;
            var subsectionLabel = formGroupKey;

            if (sourcePage?.Columns != null)
            {
                var ordinal = 1;
                foreach (var col in sourcePage.Columns)
                {
                    foreach (var fg in col.FormGroups ?? new List<FormGroupConfig>())
                    {
                        if (col.Key?.Equals(columnKey, StringComparison.OrdinalIgnoreCase) == true
                            && fg.Key?.Equals(formGroupKey, StringComparison.OrdinalIgnoreCase) == true)
                        {
                            subsectionOrdinal = ordinal;
                            subsectionLabel = $"Subsection {ordinal}";
                        }
                        ordinal++;
                    }
                }
            }

            var pageOptions = new List<object>();
            foreach (var page in form.PagesConfig ?? new List<PageConfig>())
            {
                if (page.Name?.Equals(sourcePageName, StringComparison.OrdinalIgnoreCase) == true)
                {
                    continue;
                }

                var pageLabel = string.IsNullOrEmpty(page.PageTitle) ? page.Name : page.PageTitle;
                pageOptions.Add(new { key = page.Name, text = pageLabel });
            }

            logWriter?.LogInfo(
                $"GetMoveFormGroupQuery: subsectionOrdinal={subsectionOrdinal}, eligibleTargetPageCount={pageOptions.Count}",
                "GetMoveFormGroupQuery",
                "GetAsync");

            var result = new { Id = id, SubsectionLabel = subsectionLabel, TargetPageId = "", pageOptions };
            return JsonConvert.SerializeObject(result);
        }
    }
}
