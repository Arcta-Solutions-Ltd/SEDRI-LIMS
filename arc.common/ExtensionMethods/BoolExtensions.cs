namespace arc.common.ExtensionMethods
{
    /// <summary>
    /// Contains set of static methods to help with boolean manipulation.
    /// </summary>
    public static class BoolExtensions
    {
        /// <summary>
        /// Returns 'Yes' if boolean is true and 'No' if it is false.
        /// </summary>
        /// <param b="bool">Boolean value</param>
        /// <returns>Returns 'Yes' if boolean is true and 'No' if it is false.</returns>
        public static string ToYesNo(this bool b)
        {
            return b ? "Yes" : "No";
        }
    }
}
