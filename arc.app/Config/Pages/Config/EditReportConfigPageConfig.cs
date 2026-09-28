using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditReportConfigPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{  
                            name: 'editreportconfigpage',
                            pageTitle: '@ConEdiM@',
                            text: '@ConEdiL@',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'title', type: 'singleline', label: '@GenTit@', required: true },
                                                { id: 'enabled', type: 'toggle', label: '@GenEna@', required: true, value: 'Yes', defaultValue: 'Yes' }
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
