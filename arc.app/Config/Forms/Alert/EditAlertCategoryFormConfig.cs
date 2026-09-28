using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditAlertCategoryFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editalertcategoryform',
                        viewTitle: 'Edit a new alert category.',
                        saveEvent: 'editalertcategory',
                        suppressRecordView: true,
                        initialQuery: 'editalertcategory',
                        pages: [ 'editalertcategorypage']
                    }";

            return form;
        }
    }
}
