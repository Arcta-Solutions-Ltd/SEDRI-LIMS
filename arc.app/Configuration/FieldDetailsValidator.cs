using arc.app.Common;
using arc.common.ExtensionMethods;
using arc.common.Models.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace arc.app.Configuration
{
    /// <summary>
    /// Validates field configuration details for addfield and editfield events,
    /// including list requirements and fieldgrid column definitions.
    /// </summary>
    internal class FieldDetailsValidator : ISpecialValidator
    {
        private readonly string _message;

        /// <summary>
        /// Creates a validator for the supplied field configuration payload.
        /// </summary>
        /// <param name="message">JSON payload describing the field to add or edit.</param>
        public FieldDetailsValidator(string message)
        {
            _message = message;
        }

        /// <summary>
        /// Validates the field configuration and returns a translation key when validation fails.
        /// </summary>
        /// <returns>Empty string when valid; otherwise a translation key such as @ConAfi@.</returns>
        public string ValidateMessage()
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
            var data = JsonConvert.DeserializeObject<EditFieldModel>(_message, settings);

            if (data.TypeId == 453 || data.TypeId == 454 || data.TypeId == 151)
            {
                if (data.List == 0)
                {
                    return "@ConAli@";
                }

                if (!string.IsNullOrWhiteSpace(data.ParentList))
                {
                    if (!string.IsNullOrWhiteSpace(data.FieldId) &&
                        string.Equals(data.ParentList, data.FieldId, StringComparison.OrdinalIgnoreCase))
                    {
                        return "@ConParVal@";
                    }
                }
            }

            if (data.TypeId == 149)
            {
                if (string.IsNullOrWhiteSpace(data.ContentTypeIds))
                {
                    return "@ConConE@";
                }
            }

            if (data.TypeId == 459)
            {
                if (data.FieldGrid == null || data.FieldGrid.Count() == 0)
                {
                    return "@ConAtl@";
                }

                var gridIdList = new List<string>();
                foreach (var gridLine in data.FieldGrid)
                {
                    if (string.IsNullOrWhiteSpace(gridLine.GridWidth))
                    {
                        return "@ConAfi@";
                    }
                    if (string.IsNullOrWhiteSpace(gridLine.GridId))
                    {
                        return "@ConAna@";
                    }
                    if (string.IsNullOrWhiteSpace(gridLine.GridType))
                    {
                        return "@ConAtyA@";
                    }
                    if (gridIdList.Contains(gridLine.GridId.ToLower()))
                    {
                        return "@ConGri@";
                    }
                    if (gridLine.GridType.RequiresListOption() && string.IsNullOrWhiteSpace(gridLine.GridOption))
                    {
                        return "@ConAliA@";
                    }
                    gridIdList.Add(gridLine.GridId.ToLower());
                }
            }

            return "";
        }
    }
}
