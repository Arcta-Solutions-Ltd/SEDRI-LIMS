using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddIqcTestFormConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                name: 'addiqctestform',
                viewTitle: '@QuaAddB@.',
                saveEvent: 'addiqctest',
                initialQuery: 'addiqctestquery',
                suppressRecordView: true,
                pages: [ 'selectiqctestprofilepage', 'selectqcorganismspage' ]
            }";
        }
    }
}
