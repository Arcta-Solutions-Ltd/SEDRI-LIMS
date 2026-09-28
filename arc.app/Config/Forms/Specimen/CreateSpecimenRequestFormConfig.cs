using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class CreateSpecimenRequestFormConfig : IDefinition
    {
        public string Get()
        {
            var form =
                @"{
                    name: 'createspecimenrequestform',
                    title: '@SpeAddJ@',
                    singleItemName: 'specimen',
                    initialQuery: 'blankalltestselection',
                    saveevent: 'remotespecimen',
                    startstate: '',
                    collapsible: true,
                    expanded: false,
                    recordView: 'specimenrecordview',
                    configurable: 'Yes',
                    suppressRecordView: true,
                    pages: ['patientsearchpage','patientsearchresultspage','patientdetailspage','patientaddresspage', 'advancespecimendetailspage', 'specimenattributes', 'specimentimings', 'testselectionpage'],
                    rules: [
                        { Outcome: 'visible', page: 'patientdetailspage', state: 'newpatient' },
                        { Outcome: 'visible', page: 'patientaddresspage', state: 'newpatient' }
                    ]
                }";

            return form;
        }
    }
}
