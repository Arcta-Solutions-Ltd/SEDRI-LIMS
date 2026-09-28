using arc.app.Common;

namespace arc.app.Config.Events
{
    /// <summary>
    /// Configuration class for the add instrument profile event.
    /// </summary>
    internal class AddInstrumentProfileEventConfig : IDefinition
    {
        /// <summary>
        /// Generates the JSON string for the add instrument profile event.
        /// </summary>
        /// <returns>A JSON string that represents the event configuration.</returns>
        public string Get()
        {
            return @"{ 
                        EventName: 'addinstrumentprofile',
                        Description: '@InsAddA@',
                        EventType: 'special',
                        Topic: 'Instruments',
                        TableName: 'Configs',
                        ValidationRules: [
                            { field: 'InstrumentName', rule: 'required', message: '@InsInsE@'},
                            { field: 'InterfaceTypeId', rule: 'required', message: '@InsInsF@'}
                        ],
                    }";
        }
    }
}


