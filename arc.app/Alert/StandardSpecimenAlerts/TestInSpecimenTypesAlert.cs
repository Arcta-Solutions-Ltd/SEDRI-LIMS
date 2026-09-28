using arc.common.Models.Alert;
using arc.common.Models.Specimen;
using arc.domain.Tests;
using System.Collections.Generic;
using System.Linq;

namespace arc.app.Alert.StandardSpecimenAlerts
{
    internal class TestInSpecimenTypesAlert
    {
        public bool CheckAlertConditionsMatch(StandardSpecimenAlertModel alert, SpecimenModel specimen, List<Test> tests)
        {
            var specimenTypesList = alert.SpecimenTypes.Split(",").ToList();
            var testList = alert.TestFormNames.Split(",").ToList();
            var testsFound = new List<string>();
            var testNotFound = false;
            var returnValue = false;

            if (specimenTypesList.Contains(specimen.SpecimenTypeId.ToString())) {

                if (testList.Count() > 0)
                {
                    foreach (var test in testList)
                    {
                        var foundTest = tests.Any(t => t.TestName.ToLower() == test.ToLower() && t.Status.ToLower() == "complete");
                        if (foundTest)
                        {
                            testsFound.Add(test);
                        }
                        else
                        {
                            testNotFound = true;
                        }
                    }

                    if (!testNotFound)
                    {
                        returnValue = alert.NoAdditionalTest && testsFound.Count() == tests.Count();
                    } else
                    {
                        returnValue = false;
                    }
                }
            } 

            return returnValue;
        }
    }
}
