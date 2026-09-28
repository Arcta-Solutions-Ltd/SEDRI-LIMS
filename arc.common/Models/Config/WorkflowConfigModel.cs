namespace arc.common.Models.Config;
public class WorkflowConfigModel
{
    public int Id { get; set; }
    public bool IncludeCulture { get; set; } = true;
    public bool IncludeInstrument { get; set; } = true;
}
