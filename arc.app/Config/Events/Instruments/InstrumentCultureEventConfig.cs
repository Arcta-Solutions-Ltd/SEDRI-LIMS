using arc.app.Common;

namespace arc.app.Config.Events
{
    /// <summary>
    /// Configuration class for the instrument culture event.
    /// </summary>
    public class InstrumentCultureEventConfig : IDefinition
    {
        /// <summary>
        /// Generates the JSON string for the instrument culture event.
        /// </summary>
        /// <returns>A JSON string that represents the event configuration.</returns>
        public string Get()
        {
            return @"{ 
                        EventName: 'instrumentculture',
                        Description: '@InsIda@',
                        EventType: 'special',
                        Topic: 'Instruments',
                        TableName: 'Culture'
                    }";
        }
    }
}
