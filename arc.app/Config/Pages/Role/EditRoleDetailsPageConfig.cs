using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditRoleDetailsPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'roleeditdetails',
                            pageTitle: '@RolEdiA@',
                            text: '@RolEdiB@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'RoleName', type: 'singleline', label: '@GenNam@', required: true, Max: 30},
                                                { id: 'RoleDescription', type: 'singleline', label: '@GenDes@', required: true, Max: 100},
                                                { id: 'Enabled', type: 'toggle', label: '@GenEna@', required: true, value: 'Yes'}
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
