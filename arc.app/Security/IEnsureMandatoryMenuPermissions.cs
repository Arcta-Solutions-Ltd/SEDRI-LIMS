namespace arc.app.Security
{
    /// <summary>
    /// Ensures mandatory sidebar menu keys remain enabled in role menu permission payloads.
    /// </summary>
    public interface IEnsureMandatoryMenuPermissions
    {
        /// <summary>
        /// Ensures mandatory sidebar keys are Allowed=Yes in a MenuPermissions crafted JSON payload.
        /// </summary>
        /// <param name="source">The serialized Menu Permissions event payload.</param>
        /// <returns>The payload with mandatory keys corrected to Allowed=Yes.</returns>
        string EnsureInCraftedPayload(string source);
    }
}
