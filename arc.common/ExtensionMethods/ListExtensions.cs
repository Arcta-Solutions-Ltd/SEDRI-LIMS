using System.Collections.Generic;

namespace arc.common.ExtensionMethods
{
    /// <summary>
    /// Contains set of static methods to help with list manipulation.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Translates all of the items in the list of objects into a list of strings with each string representing the contents of the model.
        /// </summary>
        /// <param name="str">List of object to translate</param>
        /// <param name="delim">Delimiter to separate the attributes in the model</param>
        ///<param name="delim">String that must prefix every line in the output list of strings</param>
        /// <returns>List of strings attribute in the model divided by the delimiter specified</returns>
        public static List<string> ToDelimitedString(this IEnumerable<object> obj, string delim, string linePrefix = "")
        {
            var returnList = new List<string>();
            foreach (var item in obj)
            {
                var nextItem = $"{linePrefix}{item.ToDelimitedString(delim)}";
                returnList.Add(nextItem);
            }
            return returnList;
        }
    }
}
