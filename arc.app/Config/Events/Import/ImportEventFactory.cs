//using arc.app.Common;

//namespace arc.app.Config.Events.Import
//{
//    internal class ImportEventFactory : IDefinitionFactory
//    {
//        public IDefinition Create(string definitionName)
//        {
//            return definitionName.ToLower() switch
//            {
//                "addimportprofile" => new AddImportProfileEventConfig(),
//                "addimportprofilefield" => new AddImportProfileFieldEventConfig(),
//                "deleteimportprofile" => new DeleteImportProfileEventConfig(),
//                "deleteimportprofilefield" => new DeleteImportProfileFieldEventConfig(),
//                "editimportprofile" => new EditImportProfileEventConfig(),
//                "loadimportfile" => new LoadImportFileEventConfig(),
//                _ => null,
//            };
//        }
//    }
//}
