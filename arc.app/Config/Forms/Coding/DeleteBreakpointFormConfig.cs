using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteBreakpointFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deletebreakpointform',
                        viewTitle: 'Delete a breakpoint.',
                        saveEvent: 'deletebreakpoint',
                        initialQuery: 'breakpointbyidforedit',
                        suppressRecordView: true,
                        pages: ['deletebreakpointpage']
                    }";

            return form;
        }
    }
}
