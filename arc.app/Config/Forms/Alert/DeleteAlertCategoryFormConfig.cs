using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteAlertCategoryFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deletealertcategoryform',
                        viewTitle: 'Delete an alert category.',
                        saveEvent: 'deletealertcategory',
                        suppressRecordView: true,
                        initialQuery: 'editalertcategory',
                        pages: [ 'deletealertcategorypage']
                    }";

            return form;
        }
    }
}
