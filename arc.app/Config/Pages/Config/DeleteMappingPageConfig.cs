using arc.app.Common;

namespace arc.app.Config.Pages.Config;
internal class DeleteMappingPageConfig : IDefinition
{
    public string Get()
    {
        var page = @"{ 
                            name: 'deletemappingpage',
                            pageTitle: '@MapDel@',
                            text: '@MapDelA@.',
                            required: 'Name',
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
                                                { id: 'configname', type: 'text', label: '@GenNam@'}
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
