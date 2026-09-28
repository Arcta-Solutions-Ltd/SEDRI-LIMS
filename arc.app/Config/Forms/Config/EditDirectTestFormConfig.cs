using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditDirectTestFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editdirecttestform',
                        viewTitle: 'Edit direct test.',
                        saveEvent: 'editdirecttest',
                        suppressRecordView: true,
                        initialQuery: 'edittestquery',
                        pages: [ 'editdirecttestpage']
                    }";

            return form;
        }
    }
}
