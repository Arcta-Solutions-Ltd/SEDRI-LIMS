namespace arc.domain.Configuration.PagesConfig;

/// <summary>
/// Reserved values for the configurable "Other" list option and its companion details field.
/// </summary>
public static class OtherOptionConstants
{
    /// <summary>
    /// Synthetic list item key appended at runtime when <see cref="FieldConfig.AllowOther"/> is enabled.
    /// Not stored in the listitem table.
    /// </summary>
    public const string Key = "__ARC_OTHER__";

    /// <summary>
    /// Language tag for the "Other" option label.
    /// </summary>
    public const string OptionLabelTag = "@GenOther@";

    /// <summary>
    /// Language tag for the companion details field label.
    /// </summary>
    public const string DetailsLabelTag = "@ConOtherDetails@";
}
