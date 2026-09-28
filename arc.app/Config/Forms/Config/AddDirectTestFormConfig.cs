using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddDirectTestFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'adddirecttestform',
                        viewTitle: 'Add direct test.',
                        saveEvent: 'adddirecttest',
                        suppressRecordView: true,
                        pages: [ 'addtestpage']
                    }";

            return form;
        }
    }
}
