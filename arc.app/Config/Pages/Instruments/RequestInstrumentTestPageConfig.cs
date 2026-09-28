using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>Single page: short instructions and instrument profile selector.</summary>
internal class RequestInstrumentTestPageConfig : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{
            name: 'requestinstrumenttestpage',
            pageTitle: '@InsReqT@',
            text: '@InsReqTB@',
            required: 'InstrumentProfileId',
            requiredRule: 'and',
            columns: [
                {
                    key: 'col1',
                    fieldWidth: 'wide',
                    itemWidth: 'wide',
                    formGroups: [
                        {
                            key: 'fg1',
                            fields: [
                                { id: 'InstrumentProfileId', type: 'instrumentprofileselector', label: '@InsProNam@', required: true, placeholder: '@GenSelK@' }
                            ]
                        }
                    ]
                }
            ]
        }";
    }
}
