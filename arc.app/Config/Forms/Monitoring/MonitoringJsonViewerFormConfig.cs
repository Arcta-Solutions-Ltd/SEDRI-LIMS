using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class MonitoringJsonViewerFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'monitoringjsonviewer',
                        initialquery: 'itemscontentsquery',
                        formtype: 'singlepage',
                        saveEvent: 'viewEventDetails',
                        recordView: 'specimenrecordview',
                        suppressRecordView: false,
                        pages: ['formattedjsonviewer']
                    }";

            return form;
        }
    }
}
