using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteFieldFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deletefieldform',
                        viewTitle: 'Delete an existing field.',
                        saveEvent: 'deletefield',
                        suppressRecordView: true,
                        initialQuery: 'deletefieldquery',
                        pages: [ 'deletefieldpage']
                    }";

            return form;
        }
    }
}
