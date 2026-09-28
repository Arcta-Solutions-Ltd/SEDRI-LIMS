using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class QualityEventFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {

            return definitionName.ToLower() switch
            {
                "addiqctest" => new AddIqcTestEventConfig(),
                "addiqctestprofile" => new AddIqcTestProfileEventConfig(),
                "deleteiqcresult" => new DeleteIqcResultEventConfig(),
                "deleteiqctest" => new DeleteIqcTestEventConfig(),
                "deleteiqctestprofile" => new DeleteIqcTestProfileEventConfig(),
                "editiqcresult" => new EditIqcResultEventConfig(),
                "editiqctestprofileqcorganism" => new EditIqcTestProfileQcOrganismEventConfig(),
                "editiqctestqcorganisms" => new EditIqcTestQcOrganismsEventConfig(),
                "markiqctestcomplete" => new MarkIqcTestCompleteEventConfig(),
                "runiqctest" => new RunIqcTestEventConfig(),
                _ => null,
            };
        }
    }
}
