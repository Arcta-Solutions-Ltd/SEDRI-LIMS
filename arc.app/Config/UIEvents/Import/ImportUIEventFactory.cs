using arc.app.Common;

namespace arc.app.Config.UIEvents.Import
{
    internal class ImportUIEventFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addimportprofileuievent" => new AddImportProfileUIEventConfig(),
                "addimportprofilefielduievent" => new AddImportProfileFieldUIEventConfig(),
                "deleteimportprofileuievent" => new DeleteImportProfileUIEventConfig(),
                "deleteimportprofilefielduievent" => new DeleteImportProfileFieldUIEventConfig(),
                "editimportprofileuievent" => new EditImportProfileUIEventConfig(),
                "loadimportfileuievent" => new LoadImportFileUIEventConfig(),
                "viewimportprofileuievent" => new ViewImportProfileUIEventConfig(),
                _ => null,
            };
        }
    }
}
