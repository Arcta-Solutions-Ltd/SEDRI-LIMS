using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditStateFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editstateform',
                        viewTitle: 'Edit state.',
                        saveEvent: 'editstate',
                        suppressRecordView: true,
                        pages: [ 'editstatepage']
                    }";

            return form;
        }
    }
}
