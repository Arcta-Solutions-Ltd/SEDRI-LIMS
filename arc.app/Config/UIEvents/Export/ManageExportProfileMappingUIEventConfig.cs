using arc.app.Common;

namespace arc.app.Config.UIEvents.Export
{
    /// <summary>
    /// UI event triggered from the export profile list view and record view when the user
    /// clicks the "Manage Mapping" button. Opens the crafted Manage Mapping form.
    /// </summary>
    internal class ManageExportProfileMappingUIEventConfig : IDefinition
    {
        /// <summary>
        /// Returns the UI event configuration as a JSON string.
        /// </summary>
        public string Get()
        {
            var newEvent = @"{
                        name: 'manageexportprofilemappinguievent',
                        description: 'Manage the JSON or XML mapping for an export profile',
                        type: 'form',
                        action: 'manageexportprofilemappingform'
                    }";

            return newEvent;
        }
    }
}
