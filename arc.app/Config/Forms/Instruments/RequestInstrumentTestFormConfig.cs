using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>Form: request a pending instrument result for the current specimen, culture, or test context.</summary>
internal class RequestInstrumentTestFormConfig : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{
            name: 'requestinstrumenttestform',
            viewTitle: '@InsReqT@',
            saveEvent: 'requestinstrumenttest',
            suppressRecordView: true,
            InitialQuery: 'requestinstrumenttestforminitialquery',
            pages: ['requestinstrumenttestpage']
        }";
    }
}
