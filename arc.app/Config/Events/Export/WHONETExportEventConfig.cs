using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class WHONETExportEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'whonetexport',
                        Description: '@ExpGenA@',
                        EventType : 'export',
                        Topic : 'Export',
                        TableName: 'Export',
                        ExportQuery: 'astlistwide'
                    }";
        }
    }
}
