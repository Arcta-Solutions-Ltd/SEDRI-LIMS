using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditIqcTestProfileQcOrganismEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                EventName: 'editiqctestprofileqcorganism', 
                Description: '@QuaEdi@',
                EventType : 'special', 
                Topic : 'Quality'
            }";
        }
    }
}
