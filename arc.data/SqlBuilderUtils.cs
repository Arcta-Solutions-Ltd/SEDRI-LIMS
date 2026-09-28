using System.Collections.Generic;

namespace arc.data
{
    /// <summary>
    /// Provides string helpers used when assembling SQL fragments from delimited configuration values.
    /// </summary>
    internal class SqlBuilderUtils
    {
        /// <summary>
        /// Splits a delimited string into fields, ignoring separators that fall inside a nested json object.
        /// </summary>
        /// <param name="contents">The delimited string to split.</param>
        /// <param name="separator">The character separating each field.</param>
        /// <returns>The trimmed fields in the order they appear.</returns>
        internal List<string> SplitStringIntoFields(string contents, char separator)
        {
            var insideJsonCount = 0;
            var newFieldList = new List<string>();
            var count = 0;
            var startPos = 0;
            
            foreach (var character in contents)
            {
                if (character == separator && insideJsonCount == 0)
                {
                    var newField = contents.Substring(startPos, count - startPos).Trim();
                    startPos = count+1;
                    newFieldList.Add(newField);
                }
                if (character == '{') { insideJsonCount += 1; }
                if (character == '}') { insideJsonCount -= 1; }

                count++;
            }

            var lastField = contents.Substring(startPos, count - startPos).Trim();
            newFieldList.Add(lastField);

            return newFieldList;
        }

        /// <summary>
        /// Removes the first and last character from a string, used to unwrap enclosing brackets or braces.
        /// </summary>
        /// <param name="contents">The string to unwrap.</param>
        /// <returns>The trimmed string without its first and last character.</returns>
        internal string RemoveFirstAndLast(string contents)
        {
            var returnValue = contents.Trim().Remove(0, 1);
            var pos = returnValue.Length - 1;
            return returnValue.Remove(pos, 1).Trim();
        }
    }
}
