using arc.app.Common;

namespace arc.app.Config.Mapper;

internal class InstrumentsMapperFactory : IDefinitionFactory
{
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "instrumentresultrecordviewmapper" => new Instruments.InstrumentResultRecordViewMapper(),
            "instrumentresultattachmentsviewmapper" => new Instruments.InstrumentResultAttachmentsViewMapper(),
            _ => null,
        };
    }
}
