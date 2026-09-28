using arc.app.Common;

namespace arc.app.Config.Forms
{
    /// <summary>
    /// Provides the form configuration definition for editing an existing field in a form.
    /// This form is used to modify and update field properties within a page configuration.
    /// </summary>
    internal class EditFieldFormConfig : IDefinition
    {
        /// <summary>
        /// Returns the JSON string representation of the edit field form configuration.
        /// </summary>
        /// <returns>A JSON string containing the form configuration with the save event, initial query, and page reference.</returns>
        public string Get()
        {
            var form = @"{
                        name: 'editfieldform',
                        viewTitle: 'Edit field definition.',
                        saveEvent: 'editfield',
                        suppressRecordView: true,
                        initialQuery: 'editfieldquery',
                        pages: [ 'editfieldpage']
                    }";

            return form;
        }
    }
}
