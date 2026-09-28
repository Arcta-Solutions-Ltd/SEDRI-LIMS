using arc.common.Models.Export;
using System;
using System.Collections.Generic;
using System.Linq;

namespace arc.app.Exports.ExportFieldProcessors;
internal class AntibioticSusceptibilityProcessor
{
    public string GetLine(List<WhonetAntibiotic> astResults, string element, string testMethod, string testMethodPrecedence, string whonetQualitativeValues, string includeMICComparison)
    {
        var newElement = "";
        if(Int32.TryParse(element, out var idAsInt))
        {
            if(astResults.Any(p => p.CultureId == idAsInt))
            {
                var res = astResults.Where(p => p.CultureId == idAsInt).ToList();

                var susc = new List<KeyValuePair<string, string>>();
                var methods = testMethod.Split(",");

                foreach (var method in methods)
                {
                    if(res.Any(p => p.TestMethodId.ToString() == method))
                    {
                        if (whonetQualitativeValues == "Yes")
                        {
                            susc.Add(new KeyValuePair<string, string>(method, res.FirstOrDefault(p => p.TestMethodId.ToString() == method).Susceptibility));
                        }
                        else
                        {
                            if (includeMICComparison == "Yes")
                            {
                                susc.Add(new KeyValuePair<string, string>(method, res.FirstOrDefault(p => p.TestMethodId.ToString() == method)?.MICComparison + res.FirstOrDefault(p => p.TestMethodId.ToString() == method).Measurement));
                            }
                            else
                            {
                                susc.Add(new KeyValuePair<string, string>(method, res.FirstOrDefault(p => p.TestMethodId.ToString() == method).Measurement));
                            }
                        }
                    }
                }

                var precedence = res.Any(p => p.TestMethodId.ToString() == testMethodPrecedence) ? res.FirstOrDefault(p => p.TestMethodId.ToString() == testMethodPrecedence) : null;
                var testMethodPrecedenceSusc = "";

                if(precedence != null)
                {
                    if(whonetQualitativeValues == "Yes")
                    {
                        testMethodPrecedenceSusc = precedence.Susceptibility;
                    }
                    else
                    {
                        if(includeMICComparison == "Yes")
                        {
                            testMethodPrecedenceSusc = precedence?.MICComparison + precedence.Measurement;
                        }
                        else
                        {
                            testMethodPrecedenceSusc = precedence.Measurement;
                        }
                    }
                }

                if (susc.Count > 0)
                {
                    if (susc.Count > 1 && !string.IsNullOrEmpty(testMethodPrecedence))
                    {
                        newElement = testMethodPrecedenceSusc;
                    }
                    else
                    {
                        newElement = susc[0].Value;
                    }
                }
            }
        }
        return newElement;
    }
}
