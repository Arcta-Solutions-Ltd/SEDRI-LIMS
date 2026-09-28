using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SettingsQueryFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "accessionnumberquery" => new AccessionNumberQuery(),
                "deletesettingquery" => new DeleteSettingQuery(),
                "editaccessionnumberquery" => new EditAccessionNumberQuery(),
                "editpatientreferencequery" => new EditPatientReferenceQuery(),
                "editsettingquery" => new EditSettingQuery(),
                "generalsettingsquery" => new GeneralSettingsQuery(),
                "patientreferencequery" => new PatientReferenceQuery(),
                _ => null,
            };
        }
    }
}
