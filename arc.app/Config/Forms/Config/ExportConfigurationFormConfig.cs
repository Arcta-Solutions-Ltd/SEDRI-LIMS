using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class ExportConfigurationFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'exportconfigurationform',
                        viewTitle: 'Export configuration.',
                        saveEvent: 'exportconfiguration',
                        suppressRecordView: true,
                        pages: [ 'exportconfigurationpage']
                    }";

            return form;
        }
    }
}
