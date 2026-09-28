using arc.app.Common;

namespace arc.app.Config.Forms.Coding
{
    internal class DeleteAntibioticGroupFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deleteantibioticgroupform',
                        viewTitle: 'Delete Antibiotic Group.',
                        saveEvent: 'deleteantibioticgroup',
                        suppressRecordView: true,
                        pages: ['deleteantibioticgrouppage']
                    }";

            return form;
        }
    }
}
