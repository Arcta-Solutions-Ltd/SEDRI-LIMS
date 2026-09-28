using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditCultureTestFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editculturetestform',
                        viewTitle: 'Edit culture test.',
                        saveEvent: 'editculturetest',
                        suppressRecordView: true,
                        initialQuery: 'edittestquery',
                        pages: [ 'edittestpage']
                    }";

            return form;
        }
    }
}
