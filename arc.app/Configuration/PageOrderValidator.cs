using arc.app.Common;
using arc.common.Models.Config;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration
{
    /// <summary>
    /// Validates editpages payloads before page order is persisted.
    /// Rejects changes that split a page group, move an anchor, or put groups in the wrong sequence.
    /// </summary>
    internal class PageOrderValidator : ISpecialValidatorAsync
    {
        private readonly IFormConfigDefinition _formConfigDefinition;
        private readonly ILogWriter _logWriter;
        private readonly string _message;

        internal PageOrderValidator(IFormConfigDefinition formConfigDefinition, string message, ILogWriter logWriter)
        {
            _formConfigDefinition = formConfigDefinition;
            _message = message;
            _logWriter = logWriter;
        }

        /// <summary>
        /// Validates the submitted page order against the page group invariants.
        /// </summary>
        /// <returns>An empty string when acceptable; otherwise the translation tag for the first breach.</returns>
        public async Task<string> ValidateMessageAsync()
        {
            var dataModel = JsonConvert.DeserializeObject<PageOrderModel>(_message);
            if (dataModel?.PageOrder == null || dataModel.PageOrder.Count == 0)
            {
                return "";
            }

            var formName = PageGroupUtils.ResolveFormName(dataModel.Id);
            if (string.IsNullOrWhiteSpace(formName))
            {
                _logWriter?.LogInfo($"PageOrderValidator: unusable id '{dataModel.Id}'", nameof(PageOrderValidator), nameof(ValidateMessageAsync));
                return "";
            }

            var form = await _formConfigDefinition.LoadFormAsync(formName);
            if (form?.Pages == null || form.Pages.Count == 0)
            {
                return "";
            }

            var proposedOrder = dataModel.PageOrder
                .Select(p => p.Id)
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .ToList();

            var breaches = PageGroupUtils.Validate(form, proposedOrder);
            if (breaches.Count == 0)
            {
                return "";
            }

            foreach (var breach in breaches)
            {
                _logWriter?.LogInfo(
                    $"PageOrderValidator: rejected reorder on form {formName}; reason={breach.Reason}; group={breach.GroupId}; pages=[{string.Join(", ", breach.PageNames)}]",
                    nameof(PageOrderValidator),
                    nameof(ValidateMessageAsync));
            }

            return breaches[0].MessageTag;
        }
    }
}
