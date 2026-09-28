using System.Collections.Generic;
using System.Text;

namespace arc.data.Utils;
internal class OrganismSearchWhereClauseUtil
{
    /// <summary>
    /// Returns an SQL where clause from provided seach string, used when searching for organisms
    /// </summary>
    /// <param name="searchString">The string used to search</param>
    /// <returns></returns>
    public static string GenerateWhereClause(string searchString, bool isListView = false, bool includeCode = true)
    {
        string[] textMatchList = ["%"];

        if (searchString != "%")
        {
            textMatchList = searchString.Split(" ");

            if (textMatchList.Length == 0 || (textMatchList.Length == 1 && textMatchList[0] == ""))
            {
                textMatchList[0] = "%";
            }
            else
            {
                for (int i = 0; i < textMatchList.Length; i++)
                {
                    textMatchList[i] = "'%" + textMatchList[i] + "%'";
                }
            }
        }

        var nameConditions = new List<string>();
        var synonymConditions = new List<string>();

        for (int i = 0; i < textMatchList.Length; i++)
        {
            if (IsIgnoredTerm(textMatchList[i])) { continue; }

            var nextMatch = i != textMatchList.Length - 1 ? textMatchList[i + 1] : "";

            if (IsSppTerm(nextMatch))
            {
                nameConditions.Add("LOWER(g.name) ilike " + textMatchList[i]);
            }
            else
            {
                var condition = new StringBuilder($"""
                 (LOWER(g.name) ilike {textMatchList[i]} or LOWER(s.name) ilike {textMatchList[i]} or LOWER(ss.name) ilike {textMatchList[i]} or LOWER(se.name) ilike {textMatchList[i]} or LOWER(ad.name) ilike {textMatchList[i]}
                 """);

                if (isListView)
                {
                    condition.AppendLine($"""
                         or LOWER(ord.name) ilike {textMatchList[i]} or LOWER(f.name) ilike {textMatchList[i]} or LOWER(orda.name) ilike {textMatchList[i]} or LOWER(fa.name) ilike {textMatchList[i]} 
                        """);

                    if (includeCode)
                    {
                        condition.AppendLine($" or LOWER(oc.code) ilike {textMatchList[i]}");
                    };
                };

                condition.AppendLine(")");

                nameConditions.Add(condition.ToString());
            }

            synonymConditions.Add($"LOWER(synonym) ilike {textMatchList[i]}");
        }

        if (nameConditions.Count == 0) { return ""; }

        var whereSearchClause = string.Join(" and ", nameConditions);
        var synonymWhereClause = string.Join(" and ", synonymConditions);

        return $"({whereSearchClause} or o.Id in (select distinct organismid from organismsynonyms where {synonymWhereClause})) ";
    }

    private static bool IsIgnoredTerm(string term)
    {
        return term == "%" || term == "'%%'" || IsSppTerm(term);
    }

    private static bool IsSppTerm(string term)
    {
        return term?.ToLower() == "'%spp%'" || term?.ToLower() == "'%spp.%'";
    }
}
