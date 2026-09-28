using arc.app.Common;

namespace arc.app.Config.Forms
{
    /// <summary>
    /// Configuration class for the edit instrument profile form.
    /// </summary>
    internal class EditInstrumentProfileFormConfig : IDefinition
    {
        /// <summary>
        /// Generates the JSON string for the edit instrument profile form.
        /// </summary>
        /// <returns>A JSON string that represents the form configuration.</returns>
        public string Get()
        {
            var form = @"{
                        name: 'editinstrumentprofileform',
                        viewTitle: '@InsEdi@',
                        initialQuery: 'editinstrumentprofilequery',
                        saveEvent: 'editinstrumentprofile',
                        suppressRecordView: true,
                        pages: ['editinstrumentprofilepage','instrumentconfigdetailsonepage']
                    }";

            return form;
        }
    }

}
