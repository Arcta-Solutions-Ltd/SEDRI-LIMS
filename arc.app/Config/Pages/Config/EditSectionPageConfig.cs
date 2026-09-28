using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditSectionPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{  
                            name: 'editsectionpage',
                            pageTitle: '@ConEdiN@',
                            text: '@ConEdiO@',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'name', type: 'text', label: '@GenNam@', required: true },
                                                { id: 'Description', type: 'singleline', label: '@GenDes@', required: true },
                                                { id: 'HeadingText', type: 'singleline', label: '@ConHea@' },
                                                { id: 'format', type: 'combobox', label: '@GenForB@', required: true, multiselect: false, placeholder: '@GenSelN@', optionsName: 'formatlist' },
                                                { id: 'source', type: 'combobox', label: '@ConSou@', required: true }
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
