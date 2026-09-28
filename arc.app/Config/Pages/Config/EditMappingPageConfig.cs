using arc.app.Common;

namespace arc.app.Config.Pages.Config;
internal class EditMappingPageConfig : IDefinition
{
    public string Get()
    {
        var page = @"{  
                            name: 'editmappingpage',
                            pageTitle: '@MapEdi@',
                            text: '@MapEdiA@',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Name', type: 'singleline', label: '@GenNam@', required: true },
                                                { id: 'Mapping', type: 'fieldgrid', label: '@MapTit@', required: true, gridfields: [
                                                        { id: 'beforemappingvalue', gridtitle: '@MapBef@', type: 'singleline', width: 'medium'  },
                                                        { id: 'aftermappingvalue', gridtitle: '@MapAft@',type: 'singleline', width: 'medium' }
                                                    ]
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

        return page;
    }
}
