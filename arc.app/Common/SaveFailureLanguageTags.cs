namespace arc.app.Common;

/// <summary>
/// Language tags returned to the client when a queued event save fails after validation.
/// Detailed exception text is written to logs and the queue error column only.
/// </summary>
public static class SaveFailureLanguageTags
{
    /// <summary>Generic user-facing tag when any event save fails (entity-neutral).</summary>
    public const string GenericSaveFailure = "@SavF@";
}
