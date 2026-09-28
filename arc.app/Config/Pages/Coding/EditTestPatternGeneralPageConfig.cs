using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditTestPatternGeneralPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'edittestpatterngeneralpage',
                            pageTitle: '@TesEdiB@',
                            text: '@TesEdiC@.',
                            required: 'testpatternname,hostid',
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
                                                { id: 'testpatternname', type: 'singleline', label: '@GenNam@', required: true, Max: 80 },
                                                { id: 'hostid', type: 'dropdown', label: '@GenHos@', optionsName: 'host', dynamic: true, required: true },
                                                { id: 'specimentypeid', type: 'dropdown', multiselect: true, label: '@SpeSpeB@', optionsName: 'specimentype'},
                                                { id: 'makedefault', type: 'toggle', label: '@GenDef@', required: true }
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
