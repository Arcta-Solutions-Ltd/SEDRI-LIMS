using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class CreateSpecimenRequestForPatientFormConfig : IDefinition
    {
        public string Get()
        {
            var form =
                @"{
                    name: 'createspecimenrequestforpatientform',
                    title: '@SpeAddJ@',
                    singleItemName: 'specimen',
                    initialQuery: 'blankalltestselectionwithpatientref',
                    saveevent: 'remotespecimen',
                    startstate: '',
                    collapsible: true,
                    expanded: false,
                    recordView: 'specimenrecordview',
                    configurable: 'Yes',
                    suppressRecordView: true,
                    defaultView: 'specimens',
                    pages: ['advancespecimendetailspage', 'specimenattributes', 'specimentimings', 'testselectionpage']
                }";

            return form;
        }
    }
}
