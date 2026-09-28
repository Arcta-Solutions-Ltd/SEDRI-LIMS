using arc.app.Common;
using arc.domain.Configuration.ViewConfig.DiaryViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.DiaryViews
{
    public class DiaryFactory : IDiaryFactory
    {
        public DiaryConfig Create(string definitionName)
        {
            IDefinition view = definitionName.ToLower() switch
            {
                "patientdiary" => new PatientDiaryConfig(),
                "specimendiary" => new SpecimenDiaryConfig(),
                _ => new SpecimenDiaryConfig(),
            };

            var def = view.Get();
            return JsonConvert.DeserializeObject<DiaryConfig>(def); ;
        }
    }
}
