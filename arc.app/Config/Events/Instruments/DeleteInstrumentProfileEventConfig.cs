using arc.app.Common;

namespace arc.app.Config.Events
{
    /// <summary>
    /// Configuration class for the delete instrument profile event.
    /// </summary>
    internal class DeleteInstrumentProfileEventConfig : IDefinition
    {
        /// <summary>
        /// Generates the JSON string for the delete instrument profile event.
        /// </summary>
        /// <returns>A JSON string that represents the event configuration.</returns>
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteinstrumentprofile',
                        Description: '@InsDelA@',
                        EventType: 'special',
                        Topic: 'Instruments',
                        TableName: 'Configs'
                    }";
        }
    }
}
