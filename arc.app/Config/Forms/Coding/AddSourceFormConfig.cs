using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddSourceFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addsourceform',
                        viewTitle: 'Add a new source.',
                        saveEvent: 'addsource',
                        suppressRecordView: true,
                        pages: ['addsourcepage']
                    }";

            return form;
        }
    }
}
