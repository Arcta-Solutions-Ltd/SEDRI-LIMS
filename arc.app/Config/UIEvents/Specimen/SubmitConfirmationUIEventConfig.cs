using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class SubmitConfirmationUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'submitconfirmationuievent',
                        description: 'Submit Confirmation',
                        type: 'form',
                        action: 'submitconfirmationform'
                    }";

            return newEvent;
        }
    }
}
