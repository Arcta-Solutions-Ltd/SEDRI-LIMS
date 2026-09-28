using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    /// <summary>
    /// Configuration for the "View Test Record" UI event (opens test record view from the tests list).
    /// </summary>
    internal class ViewTestRecordUIEventConfig : IDefinition
    {
        /// <summary>
        /// Retrieves the configuration for the "View Test Record" UI event.
        /// </summary>
        /// <returns>A string containing the UI event configuration in JSON format.</returns>
        public string Get()
        {
            var newEvent = @"{
                        name: 'viewtestrecord',
                        description: 'View test record',
                        type: 'view-record',
                        action: 'testrecordview'
                    }";

            return newEvent;
        }
    }
}
