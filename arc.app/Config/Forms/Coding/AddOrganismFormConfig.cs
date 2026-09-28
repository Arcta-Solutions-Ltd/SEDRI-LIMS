using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddOrganismFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addorganismform',
                        viewTitle: 'Add a new organism.',
                        saveEvent: 'addorganism',
                        suppressRecordView: true,
                        pages: ['selectorganismpage', 'organismlistpage', 'addcodepage']
                    }";

            return form;
        }
    }
}
