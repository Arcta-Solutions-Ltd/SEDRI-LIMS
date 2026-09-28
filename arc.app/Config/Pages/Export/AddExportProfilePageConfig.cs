using arc.app.Common;

namespace arc.app.Config.Pages.Export
{
    internal class AddExportProfilePageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'addexportprofilepage',
                            pageTitle: '@ExpProA@',
                            text: '@ExpProT@',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Name', type: 'singleline', label: '@GenNam@', required: true, Max: 50},
                                                { id: 'Description', type: 'multiline', label: '@GenDes@', required: true, Max: 250}
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
