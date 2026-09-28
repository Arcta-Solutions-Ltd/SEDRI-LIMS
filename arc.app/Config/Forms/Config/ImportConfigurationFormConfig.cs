using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class ImportConfigurationFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'importconfigurationform',
                        viewTitle: 'Import configuration.',
                        saveEvent: 'importconfiguration',
                        suppressRecordView: true,
                        pages: [ 'importconfigurationpage']
                    }";

            return form;
        }
    }
}
