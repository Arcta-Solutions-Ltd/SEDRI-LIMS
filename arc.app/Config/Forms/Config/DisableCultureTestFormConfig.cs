using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DisableCultureTestFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'disableculturetestform',
                        viewTitle: 'Disable culture test.',
                        saveEvent: 'disableculturetest',
                        suppressRecordView: true,
                        initialQuery: 'edittestquery',
                        pages: [ 'disabletestpage']
                    }";

            return form;
        }
    }
}
