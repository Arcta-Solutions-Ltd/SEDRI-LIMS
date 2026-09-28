using arc.app.Common;
using arc.common.Models.Config;
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
    /// Retrieves a single form group by id (formName|pageName|columnKey|formGroupKey).
    /// Used when opening the edit form group form.
    /// </summary>
    internal class GetFormGroupQuery : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        internal GetFormGroupQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Returns the form group as JSON with Id, Key, Rules, FieldList, fieldOptions (form page fields only, upload excluded, with type/optionsName), and effectOptions.
        /// </summary>
        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var idParam = queryFilters.Parameters?.FirstOrDefault(p => p.Key?.ToLower() == "id");
            var id = idParam?.Value ?? "";
            var idList = id.Split("|");
            if (idList.Length < 4)
            {
                var effectOpts = GetEffectOptions();
                return JsonConvert.SerializeObject(new { Id = id, Key = "", Rules = new List<FormGroupRuleModel>(), FieldList = new List<FieldListModel>(), fieldOptions = new List<object>(), effectOptions = effectOpts });
            }

            var formName = idList[0];
            var pageName = idList[1];
            var columnKey = idList[2];
            var formGroupKey = idList[3];

            var logWriter = _serviceProvider.GetService<ILogWriter>();
            logWriter?.LogInfo($"GetFormGroupQuery: formName={formName}, pageName={pageName}, columnKey={columnKey}, formGroupKey={formGroupKey}", "GetFormGroupQuery", "GetAsync");

            var formConfigDefinition = _serviceProvider.GetService<IFormConfigDefinition>();
            var form = await formConfigDefinition.LoadFormAsync(formName);
            var page = form?.PagesConfig?.FirstOrDefault(p => p.Name?.Equals(pageName, StringComparison.OrdinalIgnoreCase) == true);

            FormGroupConfig formGroup = null;
            if (page?.Columns != null)
            {
                foreach (var col in page.Columns)
                {
                    if (col.Key?.Equals(columnKey, StringComparison.OrdinalIgnoreCase) != true) continue;
                    formGroup = col.FormGroups?.FirstOrDefault(fg => fg.Key?.Equals(formGroupKey, StringComparison.OrdinalIgnoreCase) == true);
                    if (formGroup != null) break;
                }
            }

            var (fieldOptions, uploadExcludedCount) = BuildFieldOptions(form);

            if (formGroup == null)
            {
                logWriter?.LogInfo($"GetFormGroupQuery: BuildFieldOptions returned {fieldOptions.Count} options, excluded {uploadExcludedCount} upload field(s)", "GetFormGroupQuery", "GetAsync");
                var effectOpts = GetEffectOptions();
                return JsonConvert.SerializeObject(new { Id = id, Key = formGroupKey, Rules = new List<FormGroupRuleModel>(), FieldList = new List<FieldListModel>(), fieldOptions, effectOptions = effectOpts });
            }

            var rules = (formGroup.Rules ?? new List<RuleConfig>()).Select(r => new FormGroupRuleModel
            {
                Effect = r.Effect,
                Field = r.Field,
                Rule = r.Rule,
                Value = r.Value
            }).ToList();

            var fieldList = (formGroup.Fields ?? new List<FieldConfig>()).Select(f => new FieldListModel { Label = f.Label ?? f.Id, Value = f.Id }).ToList();

            var effectOptions = GetEffectOptions();

            logWriter?.LogInfo($"GetFormGroupQuery: returned {fieldOptions.Count} field options, {effectOptions.Count} effect options, excluded {uploadExcludedCount} upload field(s)", "GetFormGroupQuery", "GetAsync");

            var result = new { Id = id, Key = formGroup.Key, Name = formGroup.Key, Rules = rules, FieldList = fieldList, fieldOptions, effectOptions };

            return JsonConvert.SerializeObject(result);
        }

        private static List<object> GetEffectOptions()
        {
            return new List<object>
            {
                new { id = "visible", label = "@GenVis@" }
            };
        }

        /// <summary>
        /// Builds field options for the RulesEditor from form page fields only.
        /// Excludes initial query fields, upload-type fields, and fieldgrid-type fields. Includes type and optionsName for dropdown/combobox/radio.
        /// </summary>
        /// <param name="form">The form configuration.</param>
        /// <returns>Tuple of (field options list, count of upload fields excluded).</returns>
        private static (List<object> options, int uploadExcluded) BuildFieldOptions(FullFormConfig form)
        {
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var fieldOptions = new List<object>();
            var uploadExcludedCount = 0;

            var pageFields = form?.GetFieldsForForm() ?? new List<FieldConfig>();
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
                    continue;

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

            return (fieldOptions, uploadExcludedCount);
        }
    }
}
