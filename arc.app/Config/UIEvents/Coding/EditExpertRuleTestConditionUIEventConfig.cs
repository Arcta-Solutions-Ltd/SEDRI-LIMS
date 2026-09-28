using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    /// <summary>
    /// UI event configuration for editing an expert rule test condition from the record view.
    /// </summary>
    internal class EditExpertRuleTestConditionUIEventConfig : IDefinition
    {
        /// <summary>
        /// Returns the JSON UI event definition for the edit expert rule test condition form.
        /// </summary>
        /// <returns>JSON string defining the UI event.</returns>
        public string Get()
        {
            var newEvent =
                @"{
                        name: 'editexpertruletestconditionuievent',
                        description: 'Edit Rule Test Condition',
                        type: 'form',
                        action: 'editexpertruletestconditionform'
                    }";

            return newEvent;
        }
    }
}
