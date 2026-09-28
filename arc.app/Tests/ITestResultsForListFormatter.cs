using arc.common.Models.Tests;
using arc.domain.Tests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Tests;

/// <summary>
/// Formats test list rows for callout/display: resolves save-event display rules, hydrates field order from
/// the full form definition (<see cref="global::arc.app.Configuration.IFormConfigDefinition.LoadFormAsync"/>), reorders stored
/// JSON object keys, and runs <see cref="Common.IJsonDataFormatter"/>.
/// </summary>
public interface ITestResultsForListFormatter
{
    /// <summary>
    /// Mutates <see cref="Test.TestDescription"/> and <see cref="Test.TestResults"/> using form layout order and event display metadata.
    /// </summary>
    /// <param name="row">Direct or culture test row (e.g. <see cref="Test"/> or <see cref="TestList"/>).</param>
    /// <param name="fieldOrderCache">
    /// Optional cache keyed by <see cref="Test.TestName"/> to avoid repeated <c>LoadFormAsync</c> within one operation;
    /// when null, each call loads the form for that row.
    /// </param>
    /// <param name="applyYesNoUiAliases">When true, replaces literal Yes/No in raw <c>TestResults</c> with UI tag placeholders before formatting.</param>
    Task ApplyToTestAsync(Test row, Dictionary<string, FormFieldOrderCache> fieldOrderCache, bool applyYesNoUiAliases);

    /// <summary>
    /// Same as <see cref="ApplyToTestAsync"/> for <see cref="TestListResultModel"/> (active test list queries).
    /// </summary>
    /// <param name="row">Row from the active direct/culture test list.</param>
    /// <param name="fieldOrderCache">Optional per-operation cache keyed by test name.</param>
    /// <param name="applyYesNoUiAliases">When true, applies Yes/No placeholder substitution before formatting.</param>
    Task ApplyToListResultRowAsync(TestListResultModel row, Dictionary<string, FormFieldOrderCache> fieldOrderCache, bool applyYesNoUiAliases);

    /// <summary>
    /// Formats a culture isolate test row (updates <see cref="CultureTest.TestResults"/>); returns display label (form title or <paramref name="row"/>.<see cref="CultureTest.TestName"/>).
    /// </summary>
    /// <param name="row">Culture test from repository.</param>
    /// <param name="fieldOrderCache">Optional cache keyed by test name within one culture list build.</param>
    /// <param name="applyYesNoUiAliases">When true, applies Yes/No placeholder substitution.</param>
    /// <returns>Title suitable for grouped JSON (form title or test name).</returns>
    Task<string> FormatCultureIsolateTestAsync(CultureTest row, Dictionary<string, FormFieldOrderCache> fieldOrderCache, bool applyYesNoUiAliases);
}

/// <summary>
/// Cached field order and title from <see cref="global::arc.app.Configuration.IFormConfigDefinition.LoadFormAsync"/> for a given test/form name.
/// </summary>
public class FormFieldOrderCache
{
    /// <summary>Field ids in form layout order; empty when load failed or form has no fields.</summary>
    public IReadOnlyList<string> FieldIds { get; set; }

    /// <summary>Form title from loaded definition, if available.</summary>
    public string Title { get; set; }
}
