using arc.common.Models.Alert;
using arc.common.Models.AST;
using arc.common.Utils;
using arc.domain.Alert;
using arc.domain.Tests;
using System.Collections.Generic;
using System.Linq;

namespace arc.app.Alert
{
    public class AlertProcessor : IAlertProcessor
    {
        private readonly IConvertJsonStructureToKeyValuePair _pairConverter;

        public AlertProcessor(IConvertJsonStructureToKeyValuePair pairConverter)
        {
            _pairConverter = pairConverter;
        }

        public List<SpecimenAlert> ProcessTestList(List<Test> directTestList, List<Test> cultureTestList, List<AlertDetailsModel> alerts, int specimenId)
        {
            var alertsRaised = new List<SpecimenAlert>();

            foreach (var alert in alerts)
            {
                var fieldList = new List<TestLineModel>();
                foreach (var test in directTestList)
                {
                    if (! string.IsNullOrWhiteSpace(test.TestResults)) {
                        var newFields = _pairConverter.Convert(test.TestResults, true);
                        var newFieldsWithTestId = newFields.Select((f) => new TestLineModel { Key = f.Key, Value = f.Value, TestId = test.Id, Type = "direct" });
                        fieldList.AddRange(newFieldsWithTestId);
                    }
                }
                foreach (var test in cultureTestList)
                {
                    if (!string.IsNullOrWhiteSpace(test.TestResults))
                    {
                        var newFields = _pairConverter.Convert(test.TestResults, true);
                        var newFieldsWithTestId = newFields.Select((f) => new TestLineModel { Key = f.Key, Value = f.Value, TestId = test.Id, Type = "culture", CultureId = test.CultureId });
                        fieldList.AddRange(newFieldsWithTestId);
                    }
                }

                var foundAlert = false;
                var andValid = true;
                var testAlerts = new List<SpecimenTestAlert>();
                var cultureAlerts = new List<CultureAlert>();               
                foreach (var testline in alert.TestGrid)
                {
                    var match = fieldList.Where((f) => f.Key.ToLower() == testline.Field.ToLower() && f.Value == testline.StringValue);

                    switch (testline.Comparison.ToString())
                    {
                        case ">":
                            match = fieldList.Where((f) => f.Key.ToLower() == testline.Field.ToLower() && int.Parse(f.Value) > int.Parse(testline.StringValue));
                            break;
                        case "<":
                            match = fieldList.Where((f) => f.Key.ToLower() == testline.Field.ToLower() && int.Parse(f.Value) < int.Parse(testline.StringValue));
                            break;
                        case ">=":
                            match = fieldList.Where((f) => f.Key.ToLower() == testline.Field.ToLower() && int.Parse(f.Value) >= int.Parse(testline.StringValue));
                            break;
                        case "<=":
                            match = fieldList.Where((f) => f.Key.ToLower() == testline.Field.ToLower() && int.Parse(f.Value) <= int.Parse(testline.StringValue));
                            break;
                    }
                            if (match.Count() > 0)
                            {
                                foundAlert = true;
                                var newMatch = match.First();
                                if (!testAlerts.Any((t) => t.TestId == newMatch.TestId))
                                {
                                    if (newMatch.Type == "direct")
                                    {
                                        var newTestAlert = new SpecimenTestAlert { SpecimenAlertId = alert.Id, TestId = newMatch.TestId };
                                        testAlerts.Add(newTestAlert);
                                    } else                                   
                                    {
                                        var cultureMatch =  cultureAlerts.Where((c) => c.CultureId == newMatch.CultureId);

                                        var currentCulture = new CultureAlert { CultureId = newMatch.CultureId, CultureTestAlerts = new List<CultureTestAlert>()};
                                        if (cultureMatch.Count() > 0)
                                        {
                                            currentCulture = cultureMatch.First();
                                        }
                                        var newTestAlert = new CultureTestAlert { CultureAlertId = alert.Id, TestId = newMatch.TestId };
                                        currentCulture.CultureTestAlerts.Add(newTestAlert);
                                        if (cultureMatch.Count() == 0)
                                        {
                                            cultureAlerts.Add(currentCulture);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(alert.TestAndOr) && alert.TestAndOr.ToLower() == "987")
                                {
                                    andValid = false;
                                }
                            }
                    }
                    if (foundAlert && andValid)
                    {
                        var newAlert = new SpecimenAlert { AlertId = alert.Id, SpecimenId = specimenId, SpecimenTestAlerts = testAlerts, AlertTypeId = alert.AlertTypeId, TagId = alert.TagId, CultureAlerts = cultureAlerts, CultureId = cultureAlerts.Count() > 0 ? cultureAlerts.First().CultureId : 0 };
                        alertsRaised.Add(newAlert);
                    }                
            }

            return alertsRaised;
        }

        public List<SpecimenAlert> ApplyASTAlerts(List<AlertDetailsModel> alerts, List<ASTModel> astResults, int specimenId, int cultureId)
        {
            var alertsRaised = new List<SpecimenAlert>();

            foreach(var alert in alerts)
            {
                var foundAlert = false;
                var andValid = true;

                var astAlerts = new List<ASTTestAlert>();
                var cultureAlerts = new List<CultureAlert>();

                foreach (var testLine in alert.SusceptibilityGrid)
                {
                    var alertLineAntibiotics = testLine.AntibioticId.Split(',');

                    var match = astResults.Where((f) => IsMatch(f, alertLineAntibiotics) && (f.SusceptibilityId == testLine.SusceptibilityId || testLine.SusceptibilityId == 1180));

                    if (match.Count() > 0)
                    {
                        foundAlert = true;
                        var newMatch = match.First();
                        if (!astAlerts.Any((t) => t.Id == newMatch.Id))
                        {
                            var cultureMatch = cultureAlerts.Where((c) => c.CultureId == cultureId);

                            var currentCulture = new CultureAlert { CultureId = cultureId, ASTTestAlerts = new List<ASTTestAlert>() };
                            if (cultureMatch.Count() > 0)
                            {
                                currentCulture = cultureMatch.First();
                            }
                            var newTestAlert = new ASTTestAlert { CultureAlertId = alert.Id, AstId = newMatch.Id };
                            currentCulture.ASTTestAlerts.Add(newTestAlert);
                            if (cultureMatch.Count() == 0)
                            {
                                cultureAlerts.Add(currentCulture);
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(alert.TestAndOr) && alert.SusceptibilityAndOr.ToLower() == "987")
                        {
                            andValid = false;
                        }
                    }
                }
                if (foundAlert && andValid)
                {
                    var newAlert = new SpecimenAlert { AlertId = alert.Id, SpecimenId = specimenId, AlertTypeId = alert.AlertTypeId, TagId = alert.TagId, CultureAlerts = cultureAlerts, CultureId = cultureAlerts.Count() > 0 ? cultureAlerts.First().CultureId : 0 };
                    alertsRaised.Add(newAlert);
                }
            }
            return alertsRaised;
        }

        public bool IsMatch(ASTModel astResult, string[] alertLineAntibiotics)
        {
            foreach(var item in alertLineAntibiotics)
            {
                var antibioticToMatch = int.Parse(item);
                if (astResult.AntibioticId == antibioticToMatch || astResult.AntibioticGroupId == antibioticToMatch)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
