using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditIqcResultForm : IDefinition
    {
        public string Get()
        {
            return @"{
                name: 'editiqcresultform',
                viewTitle: '%@QuaEdiIqcRes@%.',
                saveEvent: 'editiqcresult',
                initialQuery: 'editiqcresult',
                suppressRecordView: true,
                pages: ['editiqcresultpage']
            }";
        }
    }
}
