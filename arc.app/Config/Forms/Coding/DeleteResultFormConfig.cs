using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteResultFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deleteresultform',
                        viewTitle: 'Delete a result.',
                        saveEvent: 'deleteresult',
                        suppressRecordView: true,
                        pages: ['deleteresultpage']
                    }";

            return form;
        }
    }
}
