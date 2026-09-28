using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    /// <summary>
    /// This class represents the configuration for the Specimen Print Preview UI event.
    /// </summary>
    internal class SpecimenPrintPreviewUIEventConfig : IDefinition
    {
        /// <summary>
        /// Retrieves the configuration for the specimen print preview UI event.
        /// </summary>
        /// <returns>A string containing the UI event configuration in JSON format.</returns>
        public string Get()
        {
            var newEvent = @"{
                        name: 'specimenprintpreviewuievent',
                        description: 'Specimen print preview',
                        type: 'printpreview',
                        action: 'specimenrecordview'
                    }";

            return newEvent;
        }
    }
}

