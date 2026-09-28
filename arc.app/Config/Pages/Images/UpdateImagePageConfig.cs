using arc.app.Common;

namespace arc.app.Config.Pages.Images
{
    /// <summary>
    /// Configuration for the update image page (form-agnostic page schema).
    /// </summary>
    internal class UpdateImagePageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{
                name: 'updateimagepage',
                pageTitle: '@ImgUpd@',
                text: '@ImgUpdD@.',
                columns: [
                    {
                        key: 'col1',
                        fieldWidth: 'wide',
                        itemWidth: 'wide',
                        formGroups: [
                            {
                                key: 'fg1',
                                fields: [
                                    { id: 'name', type: 'singleline', label: '@GenNam@', required: true, Max: 29 },
                                    { id: 'description', type: 'multiline', label: '@GenDes@', required: true, Max: 250 },
                                    { id: 'fileattachmentid', type: 'upload', label: '@GenIma@', required: true, 
                                        ContentTypes: ['image/png','image/jpeg','.png','.jpg','.jpeg'], 
                                    MultiSelect: false },
                                ]
                            }
                        ]
                    }
                ]
            }";

            return page;
        }
    }
}


