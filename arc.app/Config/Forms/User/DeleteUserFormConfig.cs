using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteUserFormConfig : IDefinition
    {
        public string Get()
        {
            var form =
                @"{
                    name: 'deleteuserform',
                    title: 'Delete a user',
                    initialQuery: 'userbyid',
                    saveEvent: 'deleteuser',
                    suppressRecordView: true,
                    pages: [ 'deleteuserpage']
                }";

            return form;
        }
    }
}
