using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddTableFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addtableform',
                        viewTitle: 'Add a new table.',
                        saveEvent: 'addtable',
                        suppressRecordView: true,
                        pages: ['addtablepage']
                    }";

            return form;
        }
    }
}
