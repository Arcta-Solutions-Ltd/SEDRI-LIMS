using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditRuleCategoryFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editrulecategoryform',
                        viewTitle: 'Edit an existing rule category.',
                        saveEvent: 'editrulecategory',
                        suppressRecordView: true,
                        pages: ['editrulecategorypage']
                    }";

            return form;
        }
    }
}
