using arc.app.Common;

namespace arc.app.Config.Forms
{
    /// <summary>
    /// Form definition for configuring when a page is visible and which workflow state the page sets.
    /// Uses pagerulesquery for the current settings, the selectable states and the page field list.
    /// </summary>
    internal class EditPageRulesFormConfig : IDefinition
    {
        /// <summary>
        /// Gets the JSON string that defines the edit page rules form.
        /// </summary>
        /// <returns>A JSON formatted form definition.</returns>
        public string Get()
        {
            var form = @"{
                        name: 'editpagerulesform',
                        viewTitle: '@ConPagVis@',
                        saveEvent: 'editpagerules',
                        initialquery: 'pagerulesquery',
                        suppressRecordView: true,
                        pages: [ 'editpagerulespage']
                    }";

            return form;
        }
    }
}
