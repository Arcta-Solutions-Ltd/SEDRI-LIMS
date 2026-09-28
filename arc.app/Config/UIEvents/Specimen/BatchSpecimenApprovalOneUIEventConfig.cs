using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    /// <summary>
    /// This class defines the UI event configuration for batch specimen approval at level one.
    /// </summary>
    internal class BatchSpecimenApprovalOneUIEventConfig : IDefinition
    {
        /// <summary>
        /// Gets the event configuration as a JSON string.
        /// </summary>
        /// <returns>A JSON string representing the event configuration.</returns>
        public string Get()
        {
            var newEvent = @"{
                        name: 'batchspecimenapprovaloneuievent',
                        description: 'Batch specimen Approval Level One',
                        type: 'form',
                        action: 'batchspecimenapprovaloneform'
                    }";

            return newEvent;
        }
    }
}
