using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteTableFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deletetableform',
                        viewTitle: 'Delete an existing table.',
                        saveEvent: 'deletetable',
                        suppressRecordView: true,
                        pages: ['deletetablepage']
                    }";

            return form;
        }
    }
}
