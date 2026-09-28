using arc.app.Common;
using arc.app.SystemConfig;
using arc.common.Models.Config;
using arc.domain.Configuration.PagesConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration
{
    /// <summary>
    /// Builds parent-link metadata for add/edit field configuration forms.
    /// </summary>
    public class FieldParentLinkUtils
    {
        private readonly IFormConfigDefinition _formConfigDefinition;
        private readonly IListRepository _listRepository;
        private readonly ILogWriter _logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldParentLinkUtils"/> class.
        /// </summary>
        /// <param name="formConfigDefinition">Form configuration loader.</param>
        /// <param name="listRepository">List metadata repository.</param>
        /// <param name="logWriter">Optional logger for installed-system diagnostics.</param>
        public FieldParentLinkUtils(IFormConfigDefinition formConfigDefinition, IListRepository listRepository, ILogWriter logWriter = null)
        {
            _formConfigDefinition = formConfigDefinition;
            _listRepository = listRepository;
            _logWriter = logWriter;
        }

        /// <summary>
        /// Builds the parent-link query result for a form page context.
        /// </summary>
        /// <param name="formName">Target form name.</param>
        /// <param name="excludeFieldId">Optional field id to exclude from parent candidates (edit context).</param>
        /// <param name="existingParentList">Existing parent field id when editing a field.</param>
        /// <returns>Parent-link metadata for the configuration UI.</returns>
        public async Task<FieldParentLinkQueryResultModel> BuildAsync(string formName, string excludeFieldId = null, string existingParentList = null)
        {
            var result = new FieldParentLinkQueryResultModel
            {
                ParentList = existingParentList ?? "",
                ChildLists = await _listRepository.GetChildListHierarchyAsync(),
            };

            var form = await _formConfigDefinition.LoadFormAsync(formName);
            if (form == null)
            {
                return result;
            }

            result.PageListFields = await BuildPageListFieldsAsync(form.GetFieldsForForm(), excludeFieldId);
            return result;
        }

        /// <summary>
        /// Returns parent field candidates for a child list on a form.
        /// </summary>
        /// <param name="pageListFields">All list-backed fields on the form.</param>
        /// <param name="parentListId">Parent list id from the child list definition.</param>
        /// <returns>Fields whose list id matches the parent list id.</returns>
        public static List<PageListFieldOptionModel> GetParentFieldCandidates(
            IEnumerable<PageListFieldOptionModel> pageListFields,
            int parentListId)
        {
            return pageListFields
                .Where(f => f.ListId == parentListId)
                .OrderBy(f => f.Label, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private async Task<List<PageListFieldOptionModel>> BuildPageListFieldsAsync(
            IEnumerable<FieldConfig> fields,
            string excludeFieldId)
        {
            var pageListFields = new List<PageListFieldOptionModel>();
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var field in fields)
            {
                if (string.IsNullOrWhiteSpace(field.Id) || !seen.Add(field.Id))
                {
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(excludeFieldId) &&
                    string.Equals(field.Id, excludeFieldId, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (!IsListBackedFieldType(field.Type))
                {
                    continue;
                }

                if (field.Dynamic)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(field.OptionsName))
                {
                    continue;
                }

                var listParam = new arc.domain.Configuration.QueryFiltersConfig.QueryFilterConfig();
                listParam.AddString("Value", field.OptionsName);
                var listRecord = await _listRepository.GetListByValueAsync(listParam);
                if (listRecord == null || listRecord.Id == 0)
                {
                    _logWriter?.LogInfo(
                        $"FieldParentLinkUtils: skipping field {field.Id}, optionsName '{field.OptionsName}' did not resolve to a list row",
                        nameof(FieldParentLinkUtils),
                        nameof(BuildPageListFieldsAsync));
                    continue;
                }

                pageListFields.Add(new PageListFieldOptionModel
                {
                    FieldId = field.Id,
                    Label = string.IsNullOrWhiteSpace(field.Label) ? field.Id : field.Label,
                    ListId = listRecord.Id,
                    ListName = listRecord.Name ?? field.OptionsName,
                });
            }

            return pageListFields;
        }

        private static bool IsListBackedFieldType(string type)
        {
            return string.Equals(type, "combobox", StringComparison.OrdinalIgnoreCase)
                || string.Equals(type, "dropdown", StringComparison.OrdinalIgnoreCase);
        }
    }
}
