using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditSpecimenTypeDirectTestFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editspecimentypedirecttestform',
                        viewTitle: 'Edit specimen type direct test mapping.',
                        saveEvent: 'editspecimentypedirecttest',
                        suppressRecordView: true,
                        initialQuery: 'editdirecttestdefaultquery',
                        pages: [ 'editspecimentypedirecttestpage']
                    }";

            return form;
        }
    }
}
