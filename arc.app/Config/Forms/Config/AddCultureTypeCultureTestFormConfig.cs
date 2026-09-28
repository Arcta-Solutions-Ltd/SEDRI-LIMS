using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddCultureTypeCultureTestFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addculturetypeculturetestform',
                        viewTitle: 'Add culture type culture test mapping.',
                        saveEvent: 'addculturetypeculturetest',
                        suppressRecordView: true,
                        pages: [ 'culturetypeculturetestpage']
                    }";

            return form;
        }
    }
}
