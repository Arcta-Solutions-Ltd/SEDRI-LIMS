using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteRuleCategoryFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deleterulecategoryform',
                        viewTitle: 'Delete a rule category.',
                        saveEvent: 'deleterulecategory',
                        suppressRecordView: true,
                        pages: ['deleterulecategorypage']
                    }";

            return form;
        }
    }
}
