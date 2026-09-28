using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteStateFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deletestateform',
                        viewTitle: 'Delete state.',
                        saveEvent: 'deletestate',
                        suppressRecordView: true,
                        pages: [ 'deletestatepage']
                    }";

            return form;
        }
    }
}
