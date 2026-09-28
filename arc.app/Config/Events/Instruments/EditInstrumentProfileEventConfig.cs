using arc.app.Common;

namespace arc.app.Config.Events
{
    /// <summary>
    /// Configuration class for the edit instrument profile event.
    /// </summary>
    internal class EditInstrumentProfileEventConfig : IDefinition
    {
        /// <summary>
        /// Generates the JSON string for the edit instrument profile event.
        /// </summary>
        /// <returns>A JSON string that represents the event configuration.</returns>
        public string Get()
        {
            return @"{ 
                        EventName: 'editinstrumentprofile',
                        Description: '@InsUpd@',
                        EventType : 'special',
                        Topic : 'Instruments',
                        ValidationRules: [
                            { field: 'InterfaceTypeId', rule: 'required', message: '@InsInsF@'}
                        ]
                    }";
        }
    }
}
