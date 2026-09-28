using arc.app.Common;

namespace arc.app.Config.Forms
{
    /// <summary>
    /// Configuration class for the view instrument error details form.
    /// </summary>
    internal class ViewInstrumentErrorDetailsFormConfig : IDefinition
    {
        /// <summary>
        /// Generates the JSON string for the view instrument error details form.
        /// </summary>
        /// <returns>A JSON string that represents the form configuration.</returns>
        public string Get()
        {
            var form = @"{
                        name: 'viewinstrumenterrordetailsform',
                        initialquery: 'instrumenterrorjsoncontentsquery',
                        formtype: 'singlepage',
                        saveEvent: 'viewinstrumenterrordetails',
                        suppressRecordView: true,
                        pages: ['jsonviewer']
                    }";

            return form;
        }
    }
}
