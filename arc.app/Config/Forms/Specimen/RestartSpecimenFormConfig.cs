using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class RestartSpecimenFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                            name: 'restartspecimenform',
                            formtype: 'singlepage',
                            saveEvent: 'restartspecimen',
                            recordView: 'specimenrecordview',
                            suppressRecordView: false,
                            pages: [ 'restartspecimenpage' ]
                        }";

            return form;
        }
    }
}
