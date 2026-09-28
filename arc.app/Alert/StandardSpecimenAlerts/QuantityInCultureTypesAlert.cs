using arc.common.Models.Alert;
using arc.common.Models.Specimen;
using System.Collections.Generic;
using System.Linq;

namespace arc.app.Alert.StandardSpecimenAlerts
{
    internal class QuantityInCultureTypesAlert
    {
        public bool CheckAlertConditionsMatch(StandardSpecimenAlertModel alert, List<CultureListModel> cultures)
        {
            var cultureList = alert.CultureTypes.Split(",").ToList();
            var cultureQuantityList = alert.CultureQuantity.Split(",").ToList();
            var culturesFound = new List<string>();
            var cultureNotFound = false;
            var returnValue = false;

            if (cultureList.Count() > 0)
            {
                foreach (var culture in cultureList)
                {
                    var foundCulture = cultures.Any(c => c.TypeId.ToLower() == culture.ToLower() && cultureQuantityList.Any(sq => sq == c.SpecimenQuantityId));
                    if (foundCulture)
                    {
                        culturesFound.Add(culture);
                    }
                    else
                    {
                        cultureNotFound = true;
                    }
                }

                if ( ! cultureNotFound)
                {
                    returnValue = alert.NoAdditionalCulture && culturesFound.Count() == cultures.Count();
                }
                else
                {
                    returnValue = false;
                }
            }

            return returnValue;
        }
    }
}
