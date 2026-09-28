namespace arc.common.Models.AST;

/// <summary>
/// One parsed field from a culture isolate test's <c>TestResults</c>, with test identity for expert rule test conditions.
/// Callers typically flatten culture tests using <c>IConvertJsonStructureToKeyValuePair</c> and attach <see cref="TestName"/>.
/// </summary>
public class CultureTestFieldLine
{
    public string TestName { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public int TestId { get; set; }
    public int CultureId { get; set; }
}
