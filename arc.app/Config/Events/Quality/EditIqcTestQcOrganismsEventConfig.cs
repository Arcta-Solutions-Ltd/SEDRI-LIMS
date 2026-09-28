using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditIqcTestQcOrganismsEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                EventName: 'editiqctestqcorganisms', 
                Description: '@QuaEdiQcOrgFor@',
                EventType : 'special',
                Topic : 'Quality', 
                TableName: 'iqctests'
            }";
        }
    }
}
