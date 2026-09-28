using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class CultureTestSelectionFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'culturetestselectionform',
                        title: 'Culture Test Selection',
                        initialQuery: 'blankculturetestselection',
                        saveEvent: 'CultureTestSelection',
                        recordView: 'cultures',
                        suppressRecordView: false,
                        pages: ['testcultureselectionpage']
                    }";

            return form;
        }
    }
}
