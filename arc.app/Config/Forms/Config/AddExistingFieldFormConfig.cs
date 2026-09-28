using arc.app.Common;

namespace arc.app.Config.Forms
{
    /// <summary>
    /// Form definition for adding fields that already exist on other forms to a page by reference.
    /// Uses existingfieldlistquery for the candidate list.
    /// </summary>
    internal class AddExistingFieldFormConfig : IDefinition
    {
        /// <summary>
        /// Returns the JSON configuration string for the add existing field form.
        /// </summary>
        /// <returns>A JSON string defining the add existing field form.</returns>
        public string Get()
        {
            var form = @"{
                        name: 'addexistingfieldform',
                        viewTitle: '@ConAddEF@',
                        saveEvent: 'addexistingfield',
                        suppressRecordView: true,
                        initialQuery: 'existingfieldlistquery',
                        pages: [ 'addexistingfieldpage']
                    }";

            return form;
        }
    }
}
