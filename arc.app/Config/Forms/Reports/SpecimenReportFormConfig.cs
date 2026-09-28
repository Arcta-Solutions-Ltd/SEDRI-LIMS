using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class SpecimenReportFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'specimenreportform',
                        viewTitle: 'Specimen Report.',
                        saveEvent: 'specimenreport',
                        suppressRecordView: true,
                        pages: ['specimenreportpage']
                    }";

            return form;
        }
    }
}
