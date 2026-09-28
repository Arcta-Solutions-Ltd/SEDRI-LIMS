using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddStateFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addstateform',
                        viewTitle: 'Add state.',
                        saveEvent: 'addstate',
                        suppressRecordView: true,
                        pages: [ 'addstatepage']
                    }";

            return form;
        }
    }
}
