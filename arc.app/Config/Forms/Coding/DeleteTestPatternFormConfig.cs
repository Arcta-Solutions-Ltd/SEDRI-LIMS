using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteTestPatternFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deletetestpatternform',
                        viewTitle: 'Delete a test pattern.',
                        saveEvent: 'deletetestpattern',
                        initialQuery: 'testpatternbyid',
                        suppressRecordView: true,
                        pages: ['deletetestpatternpage']
                    }";

            return form;
        }
    }
}
