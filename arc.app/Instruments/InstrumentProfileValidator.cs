using arc.app.Common;
using Newtonsoft.Json.Linq;

namespace arc.app.Instruments
{
    /// <summary>
    /// Validates add/edit instrument profile payloads. Instrument name and machine (InstrumentMachineId) are always required.
    /// When InterfaceTypeId is the Id &amp; AST instrument list item (id 9 on the InstrumentEvent list, matching the UI visibility
    /// rules on instrumentconfigdetailsonepage), Default Growth, Organism Group, and Antibiotic Group are required.
    /// When InterfaceTypeId is the Custom instrument list item (id 10), an Export Profile (ExportProfileId) is required.
    /// </summary>
    internal class InstrumentProfileValidator : ISpecialValidator
    {
        /// <summary>
        /// InstrumentEvent list item id for &quot;Id &amp; AST Instrument&quot; (must match page rules in InstrumentConfigDetailsOnePageConfig).
        /// </summary>
        internal const string IdAndAstInterfaceTypeId = "9";

        /// <summary>
        /// InstrumentEvent list item id for &quot;Custom&quot; (must match page rules in InstrumentConfigDetailsOnePageConfig).
        /// </summary>
        internal const string CustomInterfaceTypeId = "10";

        private readonly string _message;

        public InstrumentProfileValidator(string message)
        {
            _message = message;
        }

        /// <summary>
        /// Returns a language tag if validation fails, or an empty string if valid.
        /// </summary>
        public string ValidateMessage()
        {
            if (string.IsNullOrWhiteSpace(_message))
            {
                return "";
            }

            JObject root;
            try
            {
                root = JObject.Parse(_message);
            }
            catch
            {
                return "";
            }

            if (IsMissing(root["InstrumentName"]))
            {
                return "@InsInsE@";
            }

            if (IsMissing(root["InstrumentMachineId"]))
            {
                return "@GenReqB@";
            }

            var interfaceTypeRaw = root["InterfaceTypeId"];
            var interfaceTypeId = interfaceTypeRaw?.ToString().Trim();

            if (interfaceTypeId == CustomInterfaceTypeId)
            {
                if (IsMissing(root["ExportProfileId"]))
                {
                    return "@InsValExp@";
                }

                return "";
            }

            if (interfaceTypeId != IdAndAstInterfaceTypeId)
            {
                return "";
            }

            if (IsMissing(root["DefaultGrowth"]))
            {
                return "@InsValGro@";
            }

            if (IsMissing(root["OrganismGroupId"]))
            {
                return "@InsValOrg@";
            }

            if (IsMissing(root["AntibioticGroupId"]))
            {
                return "@InsValAnt@";
            }

            return "";
        }

        private static bool IsMissing(JToken token)
        {
            if (token == null || token.Type == JTokenType.Null)
            {
                return true;
            }

            var s = token.ToString().Trim();
            return string.IsNullOrEmpty(s);
        }
    }
}
