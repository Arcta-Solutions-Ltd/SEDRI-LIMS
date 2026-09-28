using arc.app.Common;

namespace arc.app.Config.Events.Export
{
    internal class DeleteExportProfileEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteExportProfile', 
                        Description: '@ExpDel@',
                        EventType : 'special', 
                        Topic : 'Export'
                    }";
        }
    }
}
