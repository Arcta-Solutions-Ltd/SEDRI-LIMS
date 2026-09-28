using arc.common.Models.Graph;
using System.Collections.Generic;
using System.Linq;

namespace arc.data.Utils;

internal static class TotalNumberOfMatchesIncorporatingTheHierarchy
{
    public static List<GraphModel> FilterRowsBasedOnLevel(IEnumerable<GraphModel> rows, int level)
    {
        var newRows = new List<GraphModel>();
        foreach (var row in rows)
        {
            var colonCount = row.Value.Count(t => t == ':');
            if (colonCount >= level)
            {
                var newValue = ReplaceWithCorrectLevel(row.Value, level);
                var foundExistingRow = newRows.Where(r => r.Day == row.Day && r.Month == row.Month && r.Year == row.Year && r.Value == newValue);
                if (foundExistingRow.Any())
                {
                    var foundRow = foundExistingRow.First();
                    foundRow.Number += row.Number;
                }
                else
                {
                    var newRow = row;
                    newRow.Value = newValue;
                    newRows.Add(newRow);
                }
            }
        }
        return newRows;
    }

    public static string ReplaceWithLastElement(string value)
    {
        var splitValue = value.Split(":");
        return splitValue[^1].Trim();
    }

    private static string ReplaceWithCorrectLevel(string value, int level)
    {
        var splitValue = value.Split(":");
        var returnString = splitValue[0];
        for (var x = 1; x <= level; x++)
        {
            returnString += " : " + splitValue[x];
        }
        return returnString.Trim();
    }
}
