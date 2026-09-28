using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class RemoveDirectTestFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'removedirecttestform',
                        viewTitle: 'Remove a direct test.',
                        saveEvent: 'removedirecttest',
                        initialquery: 'removedirecttestquery',
                        suppressRecordView: true,
                        pages: ['removedirecttestpage']
                    }";

            return form;
        }
    }
}
