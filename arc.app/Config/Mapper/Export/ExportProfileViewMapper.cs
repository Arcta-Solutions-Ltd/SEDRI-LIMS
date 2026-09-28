using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class ExportProfileViewMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'exportprofileviewmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'Description', Value: 'Description' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'ModifiedDate', Value: 'ModifiedDate' }
                        ],
                        'Target': 
                            {
                                Sections: [
                                    {
                                        Id: 'exportprofiledetails',
                                        Title: '@ExpProF@',
                                        Fields: [
                                            { Id: 'name', Label: '@ExpProN@', Highlight: true, Value: '<:1:>' },
                                            { Id: 'description', Label: '@ExpProD@', Highlight: true, Value: '<:2:>' },
                                            { Id: 'modifieddate', Label: '@ExpProMd@', Highlight: true, Value: '<:3:>' }
                                        ]
                                       
                                    }
                                ]
                            }
                     }";
        }
    }
}
