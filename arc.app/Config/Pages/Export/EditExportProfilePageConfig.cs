using arc.app.Common;

namespace arc.app.Config.Pages.Export
{
    internal class EditExportProfilePageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'editexportprofilepage',
                            pageTitle: '@ExpProE@',
                            text: '@ExpProT@',
                            required: 'Name,Description',
                            requiredRule: 'and',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Name', type: 'singleline', label: '@GenNam@', required: true},
                                                { id: 'Description', type: 'multiline', label: '@GenDes@', required: true}
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
