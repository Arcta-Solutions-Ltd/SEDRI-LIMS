using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteIqcTestFormConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                name: 'deleteiqctestform',
                viewTitle: '@QuaDelIqcTes@',
                saveEvent: 'deleteiqctest',
                initialQuery: 'iqctestfordeletequery',
                pages: ['deleteiqctestpage']
            }";
        }
    }
}
