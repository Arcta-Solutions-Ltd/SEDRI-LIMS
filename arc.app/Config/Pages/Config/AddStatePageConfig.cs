using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class AddStatePageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'addstatepage',
                            pageTitle: '@ConAddP@',
                            text: '@ConAddR@.',
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
                                                { id: 'Name', type: 'singleline', label: '@GenNam@', required: true, placeholder: '@GenLisA@' }
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
