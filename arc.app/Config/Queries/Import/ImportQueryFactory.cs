//using arc.app.Common;

//namespace arc.app.Config.Queries.Import
//{
//   internal class ImportQueryFactory : IDefinitionFactory
//    {
//        public IDefinition Create(string definitionName)
//        {
//            return definitionName.ToLower() switch
//            {
//                "addimportprofilefieldquery" => new AddImportProfileFieldQuery(),
//                "editimportprofilequery" => new EditImportProfileQuery(),
//                "editimportprofilefieldquery" => new EditImportProfileFieldQuery(),
//                "importfilelistquery" => new ImportFileListQuery(),
//                "importprofilealreadyexistsforadd" => new ImportProfileAlreadyExistsForAddQuery(),
//                "importprofilebyidquery" => new ImportProfileByIdQuery(),
//                "importprofilelistquery" => new ImportProfileListQuery(),
//                "importprofilerecordviewquery" => new ImportProfileRecordViewQuery(),
//                "importprofileviewquery" => new ImportProfileViewQuery(),
//                _ => null,
//            };
//        }
//    }
//}
