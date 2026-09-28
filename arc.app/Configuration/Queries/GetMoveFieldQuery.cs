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
    /// Retrieves data for the Move to Form Group form: field info, page options, and form group options.
    /// Id parameter: formName|pageName|fieldId. Returns pageOptions and formGroupOptions (with parentKey = page name)
    /// to support a page-first picker defaulting to the source page.
    /// </summary>
    internal class GetMoveFieldQuery : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        internal GetMoveFieldQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Returns JSON with Id, FieldLabel, TargetPageId (source page name), pageOptions, and formGroupOptions
        /// filtered by parentKey for cascading subsection selection.
        /// </summary>
        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var idParam = queryFilters.Parameters?.FirstOrDefault(p => p.Key?.ToLower() == "id");
            var id = idParam?.Value ?? "";
            var idList = id.Split("|");
            if (idList.Length < 3)
            {
                return JsonConvert.SerializeObject(new { Id = id, FieldLabel = "", TargetPageId = "", pageOptions = new List<object>(), formGroupOptions = new List<object>() });
            }

            var formName = idList[0];
            var pageName = idList[1];
            var fieldId = idList[2];

            var logWriter = _serviceProvider.GetService<ILogWriter>();
            logWriter?.LogInfo($"GetMoveFieldQuery: formName={formName}, pageName={pageName}, fieldId={fieldId}", "GetMoveFieldQuery", "GetAsync");

            var formConfigDefinition = _serviceProvider.GetService<IFormConfigDefinition>();
            var form = await formConfigDefinition.LoadFormAsync(formName);
            if (form == null)
            {
                return JsonConvert.SerializeObject(new { Id = id, FieldLabel = fieldId, TargetPageId = pageName, pageOptions = new List<object>(), formGroupOptions = new List<object>() });
            }

            string currentPageName = null;
            string currentColumnKey = null;
            string currentFormGroupKey = null;
            string fieldLabel = fieldId;

            foreach (var page in form.PagesConfig ?? new List<PageConfig>())
            {
                if (page.Columns == null) continue;
                foreach (var col in page.Columns)
                {
                    foreach (var fg in col.FormGroups ?? new List<FormGroupConfig>())
                    {
                        var field = fg.Fields?.FirstOrDefault(f => f.Id?.Equals(fieldId, StringComparison.OrdinalIgnoreCase) == true);
                        if (field != null)
                        {
                            currentPageName = page.Name;
                            currentColumnKey = col.Key;
                            currentFormGroupKey = fg.Key;
                            fieldLabel = string.IsNullOrEmpty(field.Label) ? field.Id : field.Label;
                            break;
                        }
                    }
                    if (currentFormGroupKey != null) break;
                }
                if (currentFormGroupKey != null) break;
            }

            var targetPageId = currentPageName ?? pageName;

            var pageOptions = new List<object>();
            foreach (var page in form.PagesConfig ?? new List<PageConfig>())
            {
                var pageLabel = string.IsNullOrEmpty(page.PageTitle) ? page.Name : page.PageTitle;
                pageOptions.Add(new { key = page.Name, text = pageLabel });
            }

            var formGroupOptions = new List<object>();
            foreach (var page in form.PagesConfig ?? new List<PageConfig>())
            {
                var pageKey = page.Name;
                var pageOrdinal = 1;
                if (page.Columns == null) continue;
                foreach (var col in page.Columns)
                {
                    foreach (var fg in col.FormGroups ?? new List<FormGroupConfig>())
                    {
                        var isCurrent = page.Name?.Equals(currentPageName, StringComparison.OrdinalIgnoreCase) == true
                            && col.Key?.Equals(currentColumnKey, StringComparison.OrdinalIgnoreCase) == true
                            && fg.Key?.Equals(currentFormGroupKey, StringComparison.OrdinalIgnoreCase) == true;
                        if (!isCurrent)
                        {
                            var value = $"{formName}|{page.Name}|{col.Key}|{fg.Key}";
                            var label = $"Subsection {pageOrdinal}";
                            formGroupOptions.Add(new { key = value, text = label, parentKey = pageKey });
                        }
                        pageOrdinal++;
                    }
                }
            }

            logWriter?.LogInfo(
                $"GetMoveFieldQuery: TargetPageId={targetPageId}, pageCount={pageOptions.Count}, formGroupCount={formGroupOptions.Count}",
                "GetMoveFieldQuery",
                "GetAsync");

            var result = new { Id = id, FieldLabel = fieldLabel, TargetPageId = targetPageId, pageOptions, formGroupOptions };
            return JsonConvert.SerializeObject(result);
        }
    }
}
