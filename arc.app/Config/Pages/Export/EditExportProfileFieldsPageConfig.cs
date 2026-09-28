using arc.app.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace arc.app.Config.Pages.Export
{
    internal class EditExportProfileFieldsPageConfig:IDefinition
    {
        public string Get()
        {
            var page = @"{  
                            name: 'editexportprofilefieldspage',
                            pageTitle: '@ExpProOrderFie@',
                            text: '@ExpProOrderFieD@',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'FieldList', type: 'fieldselector', label: '@GenFie@', CanMoveEntries: true, IncludeOptions: false, CanEnable: false}
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
