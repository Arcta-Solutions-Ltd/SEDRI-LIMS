using arc.app.Common;

namespace arc.app.Config.Pages.Export
{

    internal class DeleteExportProfileFieldPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'deleteexportprofilefieldpage',
                            pageTitle: '@ExpProI@',
                            text: '@ExpProJ@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'headername', type: 'text', label: '@GenNam@', required: true}
                                            ]
                                        }
                                    ]
                                }
                            ],
                            nextButton: { show: true, buttonText: '@GenDelC@' }
                        }";

            return page;
        }
    }
}
