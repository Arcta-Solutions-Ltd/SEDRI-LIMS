using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class ViewEventDetailsFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'vieweventdetailsjson',
                        initialquery: 'queueitemjsoncontents',
                        formtype: 'singlepage',
                        saveEvent: 'viewEventDetails',
                        suppressRecordView: true,
                        pages: ['jsonviewer']
                    }";

            return form;
        }
    }
}
