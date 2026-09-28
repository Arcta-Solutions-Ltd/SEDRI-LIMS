using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditReportSectionUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editreportsectionuievent',
                        description: 'Edit report section',
                        type: 'form',
                        action: 'editreportsectionform'
                    }";

            return newEvent;
        }
    }
}
