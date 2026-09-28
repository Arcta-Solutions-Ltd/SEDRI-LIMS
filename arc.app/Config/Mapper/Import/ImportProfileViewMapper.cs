//using arc.app.Common;

//namespace arc.app.Config.Mapper.Import
//{
//    internal class ImportProfileViewMapper : IDefinition
//    {
//        public string Get()
//        {
//            return @"{  
//                        'Name': 'importprofileviewmapper', 
//                        'Type': 'Standard',
//                        'Rules': [
//                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' },
//                            { Key: '<:2:>', Type: 'Mapping', Source: 'Description', Value: 'Description' },
//                            { Key: '<:3:>', Type: 'Mapping', Source: 'LastModifiedDate', Value: 'LastModifiedDate' },
//                            { Key: '<:4:>', Type: 'Mapping', Source: 'IncludeHeaderRow', Value: 'IncludeHeaderRow' },
//                            { Key: '<:5:>', Type: 'Mapping', Source: 'TableName', Value: 'TableName' }
//                        ],
//                        'Target': 
//                            {
//                                Sections: [
//                                    {
//                                        Id: 'importprofiledetails',
//                                        Title: '@ExpProF@',
//                                        Fields: [
//                                            { Id: 'name', Label: '@ExpProN@', Highlight: false, Value: '<:1:>' },
//                                            { Id: 'description', Label: '@ExpProD@', Highlight: false, Value: '<:2:>' },
//                                            { Id: 'tablename', Label: '@GenTabA@', Highlight: false, Value: '<:5:>' },
//                                            { Id: 'includeheaderrow', Label: '@ImpInc@', Highlight: false, Value: '<:4:>' },
//                                            { Id: 'lastmodifieddate', Label: '@ExpProMd@', Highlight: false, Value: '<:3:>' }
//                                        ]
                                       
//                                    }
//                                ]
//                            }
//                     }";
//        }
//    }
//}
