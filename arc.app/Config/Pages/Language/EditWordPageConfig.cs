using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditWordPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'editwordpage',
                            pageTitle: '@LanEdiB@',
                            text: '@LanEdiA@.',
                            required: 'Name',
                            requiredRule: 'and',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Value', type: 'singleline', label: '@LanNew@', required: true },
                                                { id: 'Source', type: 'text', label: '@LanOld@', required: true }
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
