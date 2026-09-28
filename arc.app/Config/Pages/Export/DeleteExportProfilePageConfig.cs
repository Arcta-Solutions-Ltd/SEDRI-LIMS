using arc.app.Common;

namespace arc.app.Config.Pages.Export
{
    internal class DeleteExportProfilePageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'deleteexportprofilepage',
                            pageTitle: '@ExpProDelA@',
                            text: '@ExpProDelB@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'name', type: 'text', label: '@GenNam@', required: true}
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
