using arc.app.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arc.app.Config.Pages.Config;
internal class AddMappingPageConfig : IDefinition
{
    public string Get()
    {
        var page = @"{  
                            name: 'addmappingpage',
                            pageTitle: '@MapAdd@',
                            text: '@MapAddA@',
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
