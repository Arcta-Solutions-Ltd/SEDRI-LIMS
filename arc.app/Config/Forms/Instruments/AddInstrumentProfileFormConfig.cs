using arc.app.Common;

namespace arc.app.Config.Forms
{
    /// <summary>
    /// Configuration class for the add instrument profile form.
    /// </summary>
    internal class AddInstrumentProfileFormConfig : IDefinition
    {
        /// <summary>
        /// Generates the JSON string for the add instrument profile form.
        /// </summary>
        /// <returns>A JSON string that represents the form configuration.</returns>
        public string Get()
        {
            var form = @"{
                        name: 'addinstrumentprofileform',
                        viewTitle: '@InsAdd@',
                        saveEvent: 'addinstrumentprofile',
                        suppressRecordView: true,
                        pages: ['addinstrumentprofilepage','instrumentconfigdetailsonepage']
                    }";

            return form;
        }
    }

}
