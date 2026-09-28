using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class AddSectionPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{  
                            name: 'addsectionpage',
                            pageTitle: '@ConAddN@',
                            text: '@ConAddO@',
                            required: 'name, description, format, source',
                            requiredRule: 'and',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'name', type: 'singleline', label: '@GenNam@', required: true },
                                                { id: 'description', type: 'singleline', label: '@GenDes@', required: true },
                                                { id: 'headingtext', type: 'singleline', label: '@ConHea@' },
                                                { id: 'format', type: 'combobox', label: '@GenForB@', required: true, multiselect: false, placeholder: '@GenSelN@', optionsName: 'formatlist' },
                                                { id: 'source', type: 'combobox', label: '@ConSou@', required: true, optionsName: 'datasourcelist' }
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
