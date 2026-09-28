using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DisableDirectTestFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'disabledirecttestform',
                        viewTitle: 'Disable direct test.',
                        saveEvent: 'disabledirecttest',
                        suppressRecordView: true,
                        initialQuery: 'edittestquery',
                        pages: [ 'disabletestpage']
                    }";

            return form;
        }
    }
}
