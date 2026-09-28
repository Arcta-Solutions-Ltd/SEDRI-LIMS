using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteReportConfigUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deletereportconfiguievent',
                        description: 'Delete report config',
                        type: 'form',
                        action: 'deletereportconfigform'
                    }";

            return newEvent;
        }
    }
}
