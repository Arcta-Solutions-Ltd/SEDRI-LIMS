using arc.app.Common;

namespace arc.app.Config.Queries.Export
{
    internal class EditExportProfileQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'EditExportProfile',
                        'TableName': 'ExportProfile',
                        'Type': 'Special',
                        
                    }";
        }
    }
}
