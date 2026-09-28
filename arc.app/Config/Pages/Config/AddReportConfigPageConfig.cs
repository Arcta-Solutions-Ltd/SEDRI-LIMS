using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class AddReportConfigPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{  
                            name: 'addreportconfigpage',
                            pageTitle: '@ConAddL@',
                            text: '@ConAddM@',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'ReportToCloneId', type: 'combobox', label: '@ConRep@', required: true, multiselect: false, placeholder: '@ConSelD@', optionsName: 'ReportConfigList', dynamic: true, translateoptions: true },
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
