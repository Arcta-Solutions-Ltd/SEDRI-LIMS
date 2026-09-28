using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddRuleCategoryFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addrulecategoryform',
                        viewTitle: 'Add a new rule category.',
                        saveEvent: 'addrulecategory',
                        suppressRecordView: true,
                        pages: ['addrulecategorypage']
                    }";

            return form;
        }
    }
}
