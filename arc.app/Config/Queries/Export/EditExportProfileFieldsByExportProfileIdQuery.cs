using arc.app.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace arc.app.Config.Queries.Export
{
    internal class EditExportProfileFieldsByExportProfileIdQuery: IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'EditExportProfileFieldsByExportProfileId',
                        'TableName': 'ExportProfileRecord',
                        'Type': 'Special',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'string' },
                            {'Name': 'HeaderName', 'Type': 'string' }
                        ],
                        'Where' : [
                            {'Field': 'ExportProfileId', 'Comparison': '=' }
                        ]
                    }";
        }
    }
}
