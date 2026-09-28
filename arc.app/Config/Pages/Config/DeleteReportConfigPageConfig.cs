using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DeleteReportConfigPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{  
                            name: 'deletereportconfigpage',
                            pageTitle: '@ConDelN@',
                            text: '@ConDelM@',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'title', type: 'text', label: '@GenTit@' },
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
