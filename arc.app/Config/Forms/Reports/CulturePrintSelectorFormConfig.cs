using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class CulturePrintSelectorFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'cultureprintselectorform',
                        viewTitle: 'Print Selector',
                        saveEvent: 'cultureprintselector',
                        suppressRecordView: true,
                        initialquery: 'editcultureprintselectionquery',
                        pages: ['cultureprintselectorpage']
                    }";

            return form;
        }
    }
}
