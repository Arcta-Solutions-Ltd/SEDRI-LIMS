using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class RunIqcTestFormConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                name: 'runiqctestform',
                viewTitle: '@QuaEntA@',
                initialQuery: 'runiqctestquery',
                saveEvent: 'runiqctest',
                suppressRecordView: true,
                pages: ['runiqctestpage']
            }";
        }
    }
}
