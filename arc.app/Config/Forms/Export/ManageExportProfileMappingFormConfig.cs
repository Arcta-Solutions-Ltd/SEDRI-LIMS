using arc.app.Common;

namespace arc.app.Config.Forms.Export
{
    /// <summary>
    /// Form configuration that hosts the Manage Mapping crafted page. Uses the standard event
    /// pipeline (saveEvent: saveexportprofilemapping) and an initial query that loads the
    /// existing mapping plus the field options classified for the editor.
    /// </summary>
    internal class ManageExportProfileMappingFormConfig : IDefinition
    {
        /// <summary>
        /// Returns the form configuration as a JSON string.
        /// </summary>
        public string Get()
        {
            var form = @"{
                        name: 'manageexportprofilemappingform',
                        viewTitle: 'Manage mapping',
                        title: '@ExpProMap@',
                        saveEvent: 'saveexportprofilemapping',
                        initialQuery: 'exportprofilemappingforprofile',
                        suppressRecordView: true,
                        pages: ['manageexportprofilemappingpage']
                    }";

            return form;
        }
    }
}
