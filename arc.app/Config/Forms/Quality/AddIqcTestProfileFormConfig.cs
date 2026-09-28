using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddIqcTestProfileFormConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                name: 'addiqctestprofileform',
                viewTitle: '@QuaAddProA@.',
                saveEvent: 'addiqctestprofile',
                suppressRecordView: true,
                pages: ['addiqctestprofilepage']
            }";
        }
    }
}
