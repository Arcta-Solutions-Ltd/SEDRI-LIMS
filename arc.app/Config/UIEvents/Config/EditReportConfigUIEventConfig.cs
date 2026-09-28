using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditReportConfigUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editreportconfiguievent',
                        description: 'Edit report config',
                        type: 'form',
                        action: 'editreportconfigform'
                    }";

            return newEvent;
        }
    }
}
