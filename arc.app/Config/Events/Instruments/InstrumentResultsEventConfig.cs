using arc.app.Common;

namespace arc.app.Config.Events
{
    /// <summary>
    /// Configuration class for the instrument results event.
    /// </summary>
    internal class InstrumentResultsEventConfig : IDefinition
    {
        /// <summary>
        /// Generates the JSON string for the instrument results event.
        /// </summary>
        /// <returns>A JSON string that represents the event configuration.</returns>
        public string Get()
        {
            return @"{ 
                        EventName: 'instrumentresults',
                        Description: '@InsNew@',
                        EventType : 'special',
                        Topic : 'Instruments',
                        TableName: 'culture',
                        ValidationRules: [],
                    }";
        }
    }
}
