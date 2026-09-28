using arc.common.Models;
using System.Collections.Generic;

namespace arc.common.Utils
{
    public class CraftedUtils : ICraftedUtils
    {
        private readonly IJsonUtils _jsonUtils;

        public CraftedUtils(IJsonUtils jsonUtils)
        {
            _jsonUtils = jsonUtils;
        }

        public List<CraftedModel> ExtractCraftedFromJson(string message)
        {
            var craftedField = _jsonUtils.GetSingleFieldValue(message, "Crafted", true);

            var returnList = new List<CraftedModel>();

            if (craftedField != "")
            {
                craftedField = _jsonUtils.RemoveFirstAndLast(craftedField);
                var listOfCrafted = _jsonUtils.SplitStringIntoFields(craftedField, ',');


                foreach (var crafted in listOfCrafted)
                {
                    var fields = _jsonUtils.ExtractFields(crafted, true);
                    var newModel = new CraftedModel { Name = fields["Name"].Replace("\"", "").Replace("'", ""), Contents = fields["Contents"]};
                    returnList.Add(newModel);
                }
            }

            return returnList;
        }
    }
}
