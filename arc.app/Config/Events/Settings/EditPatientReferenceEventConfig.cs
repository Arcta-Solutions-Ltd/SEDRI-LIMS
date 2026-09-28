using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditPatientReferenceEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editpatientreference', 
                        Description: '@SetEdiPat@',
                        EventType : 'special', 
                        Topic : 'Settings', 
                        TableName: 'Configs'
                    }";
        }
    }
}
