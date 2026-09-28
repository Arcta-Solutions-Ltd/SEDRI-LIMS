using arc.app.Common;
using arc.common.Constants;
using arc.common.Models;
using arc.common.Models.Role;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace arc.app.Security
{
    /// <summary>
    /// Ensures mandatory sidebar menu keys remain enabled when menu permissions are saved.
    /// </summary>
    public class EnsureMandatoryMenuPermissions : IEnsureMandatoryMenuPermissions
    {
        private readonly ILogWriter _logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnsureMandatoryMenuPermissions"/> class.
        /// </summary>
        /// <param name="logWriter">Logger for installed-system diagnostics.</param>
        public EnsureMandatoryMenuPermissions(ILogWriter logWriter)
        {
            _logWriter = logWriter;
        }

        /// <inheritdoc />
        public string EnsureInCraftedPayload(string source)
        {
            var sourceData = JsonConvert.DeserializeObject<CraftedWithIdForEventModel<MenuPermissionEventModel>>(source);
            var menuCrafted = sourceData?.Crafted?.FirstOrDefault(c =>
                string.Equals(c.Name, "menupermission", StringComparison.OrdinalIgnoreCase));

            if (menuCrafted?.Contents == null)
            {
                return source;
            }

            foreach (var mandatoryKey in MenuPermissionKeys.MandatorySidebarItems)
            {
                var existing = menuCrafted.Contents.FirstOrDefault(c =>
                    string.Equals(c.Key, mandatoryKey, StringComparison.OrdinalIgnoreCase));

                if (existing == null)
                {
                    menuCrafted.Contents.Add(new CraftedSelectionsModel
                    {
                        Key = mandatoryKey,
                        Name = mandatoryKey,
                        Allowed = "Yes"
                    });

                    _logWriter.LogInfo(
                        $"Mandatory menu key '{mandatoryKey}' was missing from save payload and was added as Allowed=Yes.",
                        nameof(EnsureMandatoryMenuPermissions),
                        nameof(EnsureInCraftedPayload));
                }
                else if (!string.Equals(existing.Allowed, "Yes", StringComparison.OrdinalIgnoreCase))
                {
                    existing.Allowed = "Yes";

                    _logWriter.LogInfo(
                        $"Mandatory menu key '{mandatoryKey}' was set to Allowed=No in save payload and was corrected to Yes.",
                        nameof(EnsureMandatoryMenuPermissions),
                        nameof(EnsureInCraftedPayload));
                }
            }

            return JsonConvert.SerializeObject(sourceData);
        }
    }
}
