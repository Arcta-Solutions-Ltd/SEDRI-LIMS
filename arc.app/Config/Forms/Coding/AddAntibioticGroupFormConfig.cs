using arc.app.Common;

namespace arc.app.Config.Forms.Coding
{
    internal class AddAntibioticGroupFormConfig : IDefinition
    {
        public string Get()
        {
            var form =
                @"{
                    name: 'addantibioticgroupform',
                    title: '@AntAddA@',
                    viewTitle: 'Add a new antibiotic group.',
                    saveEvent: 'addantibioticgroup',
                    suppressRecordView: true,
                    pages: ['addantibioticgrouppage']
                }";

            return form;
        }
    }
}
