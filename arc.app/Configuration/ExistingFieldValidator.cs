using arc.app.Common;
using arc.common.ExtensionMethods;
using arc.common.Models.Config;
using arc.domain.Configuration.PagesConfig;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration
{
    /// <summary>
    /// Validates <c>addexistingfield</c> payloads before any field is placed. Rejects references
    /// whose source no longer exists, whose entity scope differs from the target page, or whose field
    /// is already on the target page.
    /// <para>
    /// The duplicate check is per page rather than per form, because a definition may legitimately
    /// appear on two pages of one form: one for entry and one read only for reference.
    /// </para>
    /// </summary>
    internal class ExistingFieldValidator : ISpecialValidatorAsync
    {
        private readonly IFormConfigDefinition _formConfigDefinition;
        private readonly IExistingFieldCatalogue _catalogue;
        private readonly ILogWriter _logWriter;
        private readonly string _message;

        private const string ClassName = nameof(ExistingFieldValidator);

        /// <summary>
        /// Creates a new <see cref="ExistingFieldValidator"/>.
        /// </summary>
        /// <param name="formConfigDefinition">Loader used to read the target form.</param>
        /// <param name="catalogue">Catalogue used to resolve each reference key.</param>
        /// <param name="message">The raw JSON payload being validated.</param>
        /// <param name="logWriter">Structured log writer.</param>
        internal ExistingFieldValidator(
            IFormConfigDefinition formConfigDefinition,
            IExistingFieldCatalogue catalogue,
            string message,
            ILogWriter logWriter)
        {
            _formConfigDefinition = formConfigDefinition;
            _catalogue = catalogue;
            _message = message;
            _logWriter = logWriter;
        }

        /// <summary>
        /// Validates every selected reference against the target page.
        /// </summary>
        /// <returns>An empty string when acceptable; otherwise the translation tag for the breach.</returns>
        public async Task<string> ValidateMessageAsync()
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var payload = JsonConvert.DeserializeObject<AddExistingFieldModel>(_message, settings);
            if (string.IsNullOrWhiteSpace(payload?.ExistingFieldIds))
            {
                return "";
            }

            var idList = (payload.Id ?? string.Empty).Split("|");
            if (idList.Length < 2 || string.IsNullOrWhiteSpace(idList[0]) || string.IsNullOrWhiteSpace(idList[1]))
            {
                _logWriter?.LogInfo(
                    $"ExistingFieldValidator: unusable id '{payload.Id}'",
                    ClassName,
                    nameof(ValidateMessageAsync));
                return "";
            }

            var formName = idList[0];
            var pageName = idList[1];

            var form = await _formConfigDefinition.LoadFormAsync(formName);
            var targetPage = form?.GetPage(pageName);
            if (targetPage == null)
            {
                _logWriter?.LogInfo(
                    $"ExistingFieldValidator: page {pageName} not found on form {formName}",
                    ClassName,
                    nameof(ValidateMessageAsync));
                return "";
            }

            var targetScope = FieldReuseScopeExtensions.ResolveScope(
                targetPage.TableName,
                form.SingleItemName,
                form.SaveEventConfig?.TableName);

            if (!FieldReuseCatalogue.IsReusableTable(targetScope))
            {
                LogFailure(payload.Id, "targetTableNotReusable", targetScope, null);
                return "@ConExiFC@";
            }

            var existingIds = targetPage.GetFieldList()
                .Where(f => f?.Id != null)
                .Select(f => f.Id)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var referenceKeys = payload.ExistingFieldIds
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            foreach (var referenceKey in referenceKeys)
            {
                var candidate = await _catalogue.ResolveAsync(referenceKey);

                if (candidate == null)
                {
                    LogFailure(referenceKey, "missing", targetScope, null);
                    return "@ConExiFD@";
                }

                if (existingIds.Contains(candidate.FieldId))
                {
                    LogFailure(referenceKey, "duplicate", targetScope, candidate.TableName);
                    return "@ConExiFB@";
                }

                if (!FieldReuseScopeExtensions.ScopesMatch(targetScope, candidate.TableName))
                {
                    LogFailure(referenceKey, "scopeMismatch", targetScope, candidate.TableName);
                    return "@ConExiFC@";
                }

                if (!FieldReuseCatalogue.IsReusable(candidate.FieldId, candidate.FieldType))
                {
                    LogFailure(referenceKey, "notReusable", targetScope, candidate.TableName);
                    return "@ConExiFC@";
                }
            }

            return "";
        }

        /// <summary>
        /// Logs a rejected reference with the resolved tables, so a rejection can be diagnosed from a
        /// production log without a debugger.
        /// </summary>
        /// <param name="referenceKey">The reference key that failed.</param>
        /// <param name="reason">Short machine readable reason.</param>
        /// <param name="targetTable">Resolved target entity table.</param>
        /// <param name="candidateTable">Resolved candidate entity table.</param>
        private void LogFailure(string referenceKey, string reason, string targetTable, string candidateTable)
        {
            _logWriter?.LogInfo(
                $"Existing field validation failed: key={referenceKey}, reason={reason}, targetTable={targetTable ?? "none"}, candidateTable={candidateTable ?? "none"}",
                ClassName,
                nameof(ValidateMessageAsync));
        }
    }
}
