using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddAlertCategoryFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addalertcategoryform',
                        viewTitle: 'Add a new alert category.',
                        saveEvent: 'addalertcategory',
                        suppressRecordView: true,
                        pages: [ 'addalertcategorypage']
                    }";

            return form;
        }
    }
}
