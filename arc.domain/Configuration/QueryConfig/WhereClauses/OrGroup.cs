using System.Collections.Generic;

namespace arc.domain.Configuration.QueryConfig.WhereClauses
{
    internal class OrGroup
    {
        IDictionary<string, string> _orGroupFieldLists; 

        internal OrGroup()
        {
            _orGroupFieldLists = new Dictionary<string, string>();
        }

        internal void Add(string group, string clause)
        {
            if (group != null && group != "")
            {
                if (_orGroupFieldLists.ContainsKey(group))
                {
                    _orGroupFieldLists[group] += " or " + clause;
                }
                else
                {
                    _orGroupFieldLists.Add(group, clause);
                }
            }
        }

        internal string GetString()
        {
            var orGroups = "";
            foreach (var orGroup in _orGroupFieldLists)
            {
                orGroups += orGroups == "" ? "(" + orGroup.Value + ")" : " and (" + orGroup.Value + ")";
            }
            return orGroups;
        }
    }
}
