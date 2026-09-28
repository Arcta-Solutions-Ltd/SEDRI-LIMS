using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddPatientReferenceTextEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addpatientreferencetext', 
                        Description: '@SetAddPatTex@',
                        EventType : 'special', 
                        Topic : 'Settings', 
                        TableName: 'Configs',
                        ValidationRules: [ { field: 'textvalue', rule: 'required', message: '@SetTexNulErr@'} ]
                    }";
        }
    }
}
