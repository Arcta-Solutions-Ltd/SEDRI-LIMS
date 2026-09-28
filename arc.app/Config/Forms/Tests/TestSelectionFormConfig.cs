using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class TestSelectionFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'testselectionform',
                        title: '@TesTesC@',
                        initialQuery: 'blanktestselection',
                        saveEvent: 'TestSelection',
                        recordView: 'specimenrecordview',
                        suppressRecordView: false,
                        pages: ['testselectionpage']
                    }";

            return form;
        }
    }
}
