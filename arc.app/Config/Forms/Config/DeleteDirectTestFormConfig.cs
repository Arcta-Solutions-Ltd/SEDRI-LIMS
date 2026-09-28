using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteDirectTestFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deletedirecttestform',
                        viewTitle: 'Delete direct test.',
                        saveEvent: 'deletedirecttest',
                        suppressRecordView: true,
                        initialQuery: 'edittestquery',
                        pages: [ 'deletetestpage']
                    }";

            return form;
        }
    }
}
