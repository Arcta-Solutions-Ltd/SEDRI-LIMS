using arc.app.Common;
using arc.app.Config.Workflows;
using arc.common.Models.Config;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace arc.app.Configuration
{
    internal class AddWorkflowEntryValidator : ISpecialValidatorAsync
    {
        private readonly IWorkflowAdapter _workflowAdapter;
        private readonly string _message;

        public AddWorkflowEntryValidator(IWorkflowAdapter workflowAdapter, string message)
        {
            _workflowAdapter = workflowAdapter;
            _message = message;
        }

        public async Task<string> ValidateMessageAsync()
        {
            var workflow = await _workflowAdapter.GetWorkflowAsync("SpecimenDefault");
            var newWorkflowEntry = JsonConvert.DeserializeObject<AddWorkflowEntryModel>(_message);

            var eventConfig = workflow.GetStep(newWorkflowEntry.EventField);

            if (eventConfig != null)
            {
                return "@ConAD@";
            }
            return "";
        }
    }
}
