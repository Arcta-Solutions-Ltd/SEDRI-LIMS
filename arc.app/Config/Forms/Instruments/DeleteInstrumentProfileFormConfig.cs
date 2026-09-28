using arc.app.Common;

namespace arc.app.Config.Forms
{
    /// <summary>
    /// Configuration class for the delete instrument profile form.
    /// </summary>
    internal class DeleteInstrumentProfileFormConfig : IDefinition
    {
        /// <summary>
        /// Generates the JSON string for the delete instrument profile form.
        /// </summary>
        /// <returns>A JSON string that represents the form configuration.</returns>
        public string Get()
        {
            var form = @"{
                        name: 'deleteinstrumentprofileform',
                        viewTitle: '@InsDel@',
                        initialQuery: 'editinstrumentprofilequery',
                        saveEvent: 'deleteinstrumentprofile',
                        suppressRecordView: true,
                        pages: ['deleteinstrumentprofilepage']
                    }";

            return form;
        }
    }
}
