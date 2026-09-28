using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DHIS2ExportPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{
                            name: 'dhis2exportpage',
                            pageTitle: '@ExpDhi@',
                            text: '@ExpCon@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'StartDate', type: 'date', label: '@GenStaB@', required: true, placeholder: '@GenSel@' },
                                                { id: 'EndDate', type: 'date', label: '@GenEnd@', placeholder: '@GenSelA@' }
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
