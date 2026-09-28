using arc.app.Common;

namespace arc.app.Config.Forms;

internal class MovePatientFormConfig : IDefinition
{
    public string Get()
    {
        var form = @"{
                        name: 'movepatientform',
                        viewTitle: 'Move specimen from one patient to another.',
                        saveEvent: 'movepatient',
                        startstate: 'notrejected',
                        initialQuery: 'SpecimenByIdForCancelRequest',
                        suppressRecordView: true,
                        pages: ['patientsearchpage','patientsearchresultspage','patientdetailspage','patientaddresspage','movepatientpage'],
                        rules:
                        [
                            { Outcome: 'visible', page: 'patientdetailspage', state: 'newpatient' },
                            { Outcome: 'visible', page: 'patientaddresspage', state: 'newpatient' }
                        ]
                    }";

        return form;
    }
}
