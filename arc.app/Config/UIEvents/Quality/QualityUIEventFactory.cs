using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class QualityUIEventFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addiqcresultuievent" => new AddIqcResultUIEventConfig(),
                "addiqctestprofileuievent" => new AddIqcTestProfileUIEvent(),
                "addiqctestuievent" => new AddIqcTestUIEventConfig(),
                "deleteiqcresultuievent" => new DeleteIqcResultUIEventConfig(),
                "deleteiqctestprofileuievent" => new DeleteIqcTestProfileUIEvent(),
                "deleteiqctestuievent" => new DeleteIqctestUIEvent(),
                "editiqcresultuievent" => new EditIqcResultUIEventConfig(),
                "editiqctestprofileqcorganismuievent" => new EditIqcTestProfileQcOrganismUIEventConfig(),
                "editiqctestqcorganismsuievent" => new EditIqcTestQcOrganismsUIEventConfig(),
                "markiqctestcompleteuievent" => new MarkIqcTestCompleteUIEventConfig(),
                "runiqctestuievent" => new RunIqcTestUIEventConfig(),
                "viewiqctestprofileqcorganismuievent" => new ViewIqcTestProfileQcOrganismUIEventConfig(),
                "viewiqctestuievent" => new ViewIqcTestUIEventConfig(),
                _ => null
            };
        }
    }
}
