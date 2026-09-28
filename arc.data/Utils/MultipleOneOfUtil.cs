namespace arc.data.Utils
{
    internal class MultipleOneOfUtil
    {
        public string Get(string field, string value)
        {
            var newWhere = "";
            string[] valuesToInclude = value.Split(",");
            if (valuesToInclude.Length > 0)
            {
                newWhere = " (LOWER(s." + field + ") like '%" + valuesToInclude[0] + "%' ";
            }
            if (valuesToInclude.Length > 1)
            {
                for (var index = 1; index < valuesToInclude.Length; index++)
                {
                    newWhere += " or LOWER(s." + field + ") like '%" + valuesToInclude[index] + "%' ";
                }
            }
            return newWhere += ")";
        }
    }
}
