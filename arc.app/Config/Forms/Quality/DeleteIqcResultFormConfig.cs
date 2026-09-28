using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteIqcResultFormConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                name: 'deleteiqcresultform',
                viewTitle: '@QuaDelIqcTesRes@',
                saveEvent: 'deleteiqcresult',
                initialQuery: 'deleteiqcresultquery',
                suppressRecordView: true,
                pages: ['deleteiqcresultpage']
            }";
        }
    }
}
