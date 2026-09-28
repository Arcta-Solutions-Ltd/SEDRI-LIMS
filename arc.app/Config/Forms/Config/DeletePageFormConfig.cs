using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeletePageFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deletepageform',
                        viewTitle: 'Delete page definition.',
                        saveEvent: 'deletepage',
                        initialquery: 'editpagequery',
                        suppressRecordView: true,
                        pages: [ 'deletepagepage']
                    }";

            return form;
        }
    }
}
