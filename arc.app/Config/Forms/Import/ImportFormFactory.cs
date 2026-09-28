//using arc.app.Common;

//namespace arc.app.Config.Forms.Import
//{
//    internal class ImportFormFactory : IDefinitionFactory
//    {
//        public IDefinition Create(string definitionName)
//        {
//            return definitionName.ToLower() switch
//            {
//                "addimportprofileform" => new AddImportProfileFormConfig(),
//                "addimportprofilefieldform" => new AddImportProfileFieldFormConfig(),
//                "deleteimportprofileform" => new DeleteImportProfileFormConfig(),
//                "deleteimportprofilefieldform" => new DeleteImportProfileFieldFormConfig(),
//                "editimportprofileform" => new EditImportProfileFormConfig(),
//                "loadimportfileform" => new LoadImportFileFormConfig(),
//                _ => null,
//            };
//        }
//    }
//}
