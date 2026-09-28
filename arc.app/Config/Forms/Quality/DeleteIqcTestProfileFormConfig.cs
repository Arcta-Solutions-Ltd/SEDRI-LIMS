using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteIqcTestProfileFormConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                name: 'deleteiqctestprofileform',
                viewTitle: '@QuaDelProA@.',
                saveEvent: 'deleteiqctestprofile',
                suppressRecordView: true,
                pages: ['deleteiqctestprofilepage']
            }";
        }
    }
}
