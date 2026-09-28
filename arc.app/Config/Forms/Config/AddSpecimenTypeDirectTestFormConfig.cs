using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddSpecimenTypeDirectTestFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addspecimentypedirecttestform',
                        viewTitle: 'Add specimen type direct test mapping.',
                        saveEvent: 'addspecimentypedirecttest',
                        suppressRecordView: true,
                        pages: [ 'specimentypedirecttestpage']
                    }";

            return form;
        }
    }
}
