using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddCultureTestFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addculturetestform',
                        viewTitle: 'Add culture test.',
                        saveEvent: 'addculturetest',
                        suppressRecordView: true,
                        pages: [ 'addtestpage']
                    }";

            return form;
        }
    }
}
