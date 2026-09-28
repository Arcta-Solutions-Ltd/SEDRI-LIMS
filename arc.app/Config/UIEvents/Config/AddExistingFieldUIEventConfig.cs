using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    /// <summary>
    /// UI event that opens the Add Existing Field form from a form group on Define Page Contents.
    /// </summary>
    internal class AddExistingFieldUIEventConfig : IDefinition
    {
        /// <summary>
        /// Returns the JSON configuration string for the add existing field UI event.
        /// </summary>
        /// <returns>A JSON string defining the add existing field UI event.</returns>
        public string Get()
        {
            var newEvent = @"{
                        name: 'addexistingfielduievent',
                        description: 'Add existing field',
                        type: 'form',
                        action: 'addexistingfieldform'
                    }";

            return newEvent;
        }
    }
}
