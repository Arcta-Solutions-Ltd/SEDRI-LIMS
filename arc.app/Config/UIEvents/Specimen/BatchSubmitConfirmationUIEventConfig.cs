using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class BatchSubmitConfirmationUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'batchsubmitconfirmationuievent',
                        description: 'Batch submit confirmation',
                        type: 'form',
                        action: 'batchsubmitconfirmationform'
                    }";

            return newEvent;
        }
    }
}
