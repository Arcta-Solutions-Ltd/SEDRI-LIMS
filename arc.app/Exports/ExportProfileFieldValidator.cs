using arc.app.Common;
using arc.common.Models.Export;
using Newtonsoft.Json;

namespace arc.app.Exports
{
    internal class ExportProfileFieldValidator : ISpecialValidator
    {
        private readonly string _message;
        public ExportProfileFieldValidator(string message)
        {
            _message = message;
        }
        public string ValidateMessage()
        {
            var exProf = JsonConvert.DeserializeObject<AddExportProfileFieldViewModel>(_message);
            if (string.IsNullOrEmpty(exProf.Name))
            {
                return "@ExpProFielNameErr@";
            }
            else if (string.IsNullOrEmpty(exProf.Heading))
            {
                return "@ExpProFielHeadErr@";
            }
            else if (exProf.Name == "QualitativeAntibioticSusceptibility|Custom|Culture|Custom" || exProf.Name == "AntibioticMeasurement|Custom|Culture|Custom")
            {
                if (string.IsNullOrEmpty(exProf.Antibiotic))
                {
                    return "@ExpProFielAntiErr@";
                }
                else if (string.IsNullOrEmpty(exProf.TestMethod))
                {
                    return "@ExpProFielTestMethErr@";
                }
                else if (exProf.TestMethod.Contains(',') && string.IsNullOrEmpty(exProf.TestMethodPrecedence))
                {
                    return "@ExpProFielTestMethPreErr@";
                }
            }
            else if (exProf.Name == "SpecimenComments|Custom|Specimen|Mulitcolumn" || exProf.Name == "CultureComments|Custom|Culture|Mulitcolumn")
            {
                if (string.IsNullOrEmpty(exProf.CommentFormat))
                {
                    return "@ExpProFielComForErr@";
                }
                else if (string.IsNullOrEmpty(exProf.CommentType))
                {
                    return "@ExpProFielComTypeErr@";
                }
            }
            return "";
        }
    }
}
