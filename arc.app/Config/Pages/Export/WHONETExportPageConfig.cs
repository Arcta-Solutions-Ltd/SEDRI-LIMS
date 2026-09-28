using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class WHONETExportPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{
                            name: 'whonetexportpage',
                            pageTitle: '@ExpWho@',
                            text: '@ExpConA@.',
                            crafted: true,
                            nextButton: { show: false },
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'StartDate', type: 'date', label: '@GenStaB@', required: true, placeholder: '@GenSel@', Min: 'now y-3', Max: 'now d-1' },
                                                { id: 'EndDate', type: 'date', label: '@GenEnd@', placeholder: '@GenSelA@', Min: 'now y-1', Max: 'now' }
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
