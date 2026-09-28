using arc.app.Common;
using arc.app.SystemConfig;
using arc.common;
using arc.common.ExtensionMethods;
using arc.common.Models.Config;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration
{
    /// <summary>
    /// Validates parent-link configuration on add/edit field events.
    /// </summary>
    internal class FieldParentLinkValidator : ISpecialValidatorAsync
    {
        private readonly IFormConfigDefinition _formConfigDefinition;
        private readonly IListRepository _listRepository;
        private readonly ILogWriter _logWriter;
        private readonly string _message;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldParentLinkValidator"/> class.
        /// </summary>
        /// <param name="formConfigDefinition">Form configuration loader.</param>
        /// <param name="listRepository">List metadata repository.</param>
        /// <param name="message">Raw event JSON payload.</param>
        /// <param name="logWriter">Optional logger.</param>
        internal FieldParentLinkValidator(
            IFormConfigDefinition formConfigDefinition,
            IListRepository listRepository,
            string message,
            ILogWriter logWriter)
        {
            _formConfigDefinition = formConfigDefinition;
            _listRepository = listRepository;
            _message = message;
            _logWriter = logWriter;
        }

        /// <summary>
        /// Validates that a configured parent field exists on the form and uses the expected parent list id.
        /// </summary>
        /// <returns>An empty string when valid, otherwise a translation tag.</returns>
        public async Task<string> ValidateMessageAsync()
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
            };
            var data = JsonConvert.DeserializeObject<EditFieldModel>(_message, settings);
            if (data == null || data.TypeId != 453 && data.TypeId != 454)
            {
                return "";
            }

            if (data.List == 0)
            {
                return "";
            }

            var listParam = new arc.domain.Configuration.QueryFiltersConfig.QueryFilterConfig();
            listParam.AddString("metaflistid", data.List.ToString());
            var listRecord = await _listRepository.GetListByIdAsync(listParam);
            if (!listRecord.IsChildTable())
            {
                if (!string.IsNullOrWhiteSpace(data.ParentList))
                {
                    _logWriter?.LogInfo(
                        $"FieldParentLinkValidator: ParentList set on non-child list id={data.List}",
                        nameof(FieldParentLinkValidator),
                        nameof(ValidateMessageAsync));
                    return "@ConParVal@";
                }

                return "";
            }

            if (string.IsNullOrWhiteSpace(data.ParentList))
            {
                return "";
            }

            var idModel = JsonConvert.DeserializeObject<IdModel>(_message, settings);
            var formName = idModel?.Id?.Split('|').FirstOrDefault();
            if (string.IsNullOrWhiteSpace(formName))
            {
                _logWriter?.LogInfo(
                    "FieldParentLinkValidator: missing form id in payload",
                    nameof(FieldParentLinkValidator),
                    nameof(ValidateMessageAsync));
                return "@ConParVal@";
            }

            var excludeFieldId = !string.IsNullOrWhiteSpace(data.FieldId) ? data.FieldId : null;
            var utils = new FieldParentLinkUtils(_formConfigDefinition, _listRepository, _logWriter);
            var metadata = await utils.BuildAsync(formName, excludeFieldId);
            var candidates = FieldParentLinkUtils.GetParentFieldCandidates(metadata.PageListFields, listRecord.ParentListId ?? 0);
            var match = candidates.FirstOrDefault(c => c.FieldId.IsSameAs(data.ParentList));
            if (match == null)
            {
                _logWriter?.LogInfo(
                    $"FieldParentLinkValidator: parent field '{data.ParentList}' not found for child list id={data.List}, expected parent list id={listRecord.ParentListId}",
                    nameof(FieldParentLinkValidator),
                    nameof(ValidateMessageAsync));
                return "@ConParVal@";
            }

            return "";
        }
    }
}
