using System;
using arc.app.Common;
using arc.app.Configuration;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration.Queries
{
    /// <summary>
    /// Retrieves the list of field options available for configuring rules in a form.
    /// Field options come from form page fields only (initial query fields are excluded).
    /// Upload-type and fieldgrid-type fields are excluded from rules criteria.
    /// Each option includes id, label, type, and optionsName (for dropdown/combobox/radio) to support value selection.
    /// Used when opening Add Form Group (addformgroupform) or embedding RulesEditor in form context.
    /// </summary>
    internal class GetFormFieldOptionsQuery : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFormFieldOptionsQuery"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider for dependency resolution.</param>
        internal GetFormFieldOptionsQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Retrieves field options for the specified form.
        /// </summary>
        /// <param name="queryFilters">Query filters containing the "id" parameter: formName or formName|pageName.</param>
        /// <param name="queryData">Additional query configuration data (not used).</param>
        /// <returns>JSON string with fieldOptions array: [{ id, label, type, optionsName }].</returns>
        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var idParam = queryFilters.Parameters?.FirstOrDefault(p => p.Key?.ToLower() == "id");
            var id = idParam?.Value ?? "";
            var idList = id.Split("|");
            var formName = idList.Length > 0 ? idList[0] : "";
            var pageName = idList.Length > 1 ? idList[1] : null;

            var logWriter = _serviceProvider.GetService<ILogWriter>();
            logWriter?.LogInfo($"GetFormFieldOptionsQuery: formName={formName}, pageName={pageName ?? "(all)"}", "GetFormFieldOptionsQuery", "GetAsync");

            var formConfigDefinition = _serviceProvider.GetService<IFormConfigDefinition>();
            var form = await formConfigDefinition.LoadFormAsync(formName);

            var seen = new HashSet<string>(System.StringComparer.OrdinalIgnoreCase);
            var fieldOptions = new List<object>();
            var uploadExcludedCount = 0;
            var fieldgridExcludedCount = 0;

            var pageFields = form?.GetFieldsForForm() ?? new List<arc.domain.Configuration.PagesConfig.FieldConfig>();
            foreach (var f in pageFields)
            {
                if (string.IsNullOrEmpty(f.Id) || !seen.Add(f.Id))
                    continue;

                if (string.Equals(f.Type, "upload", StringComparison.OrdinalIgnoreCase))
                {
                    uploadExcludedCount++;
                    continue;
                }

                if (string.Equals(f.Type, "fieldgrid", StringComparison.OrdinalIgnoreCase))
                {
                    fieldgridExcludedCount++;
                    continue;
                }

                var label = string.IsNullOrEmpty(f.Label) ? f.Id : f.Label;
                var type = f.Type ?? "";
                var optionsName = (string.Equals(type, "dropdown", StringComparison.OrdinalIgnoreCase) ||
                                  string.Equals(type, "combobox", StringComparison.OrdinalIgnoreCase) ||
                                  string.Equals(type, "picker", StringComparison.OrdinalIgnoreCase) ||
                                  string.Equals(type, "hierarchicalpicker", StringComparison.OrdinalIgnoreCase) ||
                                  string.Equals(type, "radio", StringComparison.OrdinalIgnoreCase))
                    ? f.OptionsName ?? ""
                    : "";

                fieldOptions.Add(new { id = f.Id, label, type, optionsName });
            }

            logWriter?.LogInfo($"GetFormFieldOptionsQuery: returned {fieldOptions.Count} field options, excluded {uploadExcludedCount} upload, {fieldgridExcludedCount} fieldgrid field(s)", "GetFormFieldOptionsQuery", "GetAsync");

            var effectOptions = new List<object>
            {
                new { id = "visible", label = "@GenVis@" }
            };
            var result = new { fieldOptions, effectOptions };
            return JsonConvert.SerializeObject(result);
        }
    }
}
