using arc.app.Common;

namespace arc.app.Config.Queries.Instruments;

/// <summary>
/// Query configuration for <see cref="Instruments.InstrumentResultRecordViewByIdQuery"/>.
/// </summary>
internal class InstrumentResultRecordViewByIdQueryConfig : IDefinition
{
    public string Get()
    {
        return @"{
            'Query': 'instrumentresultrecordviewbyid',
            'TableName': 'InstrumentResults',
            'Type': 'Special',
            'Translate': true,
            'ResultMapping': 'instrumentresultrecordviewmapper'
        }";
    }
}
