using arc.app.Common;
using arc.app.Config.Mapper.Export;

namespace arc.app.Config.Mapper
{
    internal class ExportMapperFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "whonetexportmapper" => new WHONETExportMapper(),
                "exportprofileviewmapper" => new ExportProfileViewMapper(),
                "exporthistoryattachmentsviewmapper" => new ExportHistoryAttachmentsViewMapper(),
                "exporthistoryviewmapper" => new ExportHistoryViewMapper(),
                _ => null,
            };
        }
    }
}
