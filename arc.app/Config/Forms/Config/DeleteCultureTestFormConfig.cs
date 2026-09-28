using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteCultureTestFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deleteculturetestform',
                        viewTitle: 'Delete culture test.',
                        saveEvent: 'deleteculturetestconfig',
                        suppressRecordView: true,
                        initialQuery: 'edittestquery',
                        pages: [ 'deletetestpage']
                    }";

            return form;
        }
    }
}
