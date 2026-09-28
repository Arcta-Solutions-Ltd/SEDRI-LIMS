using System;
using System.Collections.Generic;
using System.Linq;

namespace arc.common.Utils
{
    public static class StringUtils
    {
        public static List<int> AllIndexesOf(this string str, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }

        public static Dictionary<string, string> AllMatchingEntriesInFieldList(this string str, string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");
            var entries = new Dictionary<string, string>();

            var list = str.Split(",");
            foreach (var entry in list)
            {
                if (entry.IndexOf('.') != -1)
                {
                    var values = GetFieldName(entry);
                    if (values[1].Trim().ToLower() == value.Trim().ToLower())
                    {
                        entries.Add(values[0].Trim(), values[1].Trim());
                    }
                }
            }
            return entries;
        }
        public static bool Is(this string self, string value)
        {
            if (self == null) return value == null;
            if(value == null) return false;
            return String.Equals(self, value, StringComparison.OrdinalIgnoreCase);

        }

        private static string[] GetFieldName(string fieldName)
        {
            if (fieldName.IndexOf(" As ") == -1)
            {
                return fieldName.Split(".");
            }
            var entries = fieldName.Split(" As ");
            return new string[]
            {
                entries[0].Split(".")[0],
                entries[1]
            };
        }

        public static List<string> SplitString(this string str, string value)
        {
            return [.. str.Split(new string[] { value }, StringSplitOptions.TrimEntries)];
        }
    }
}
