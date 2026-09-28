using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteSpecimenTypeDirectTestFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deletespecimentypedirecttestform',
                        viewTitle: 'Delete specimen type direct test mapping.',
                        saveEvent: 'deletespecimentypedirecttest',
                        suppressRecordView: true,
                        initialQuery: 'deletedirecttestdefaultquery',
                        pages: [ 'deletespecimentypedirecttestpage']
                    }";

            return form;
        }
    }
}
