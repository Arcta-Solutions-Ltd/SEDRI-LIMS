namespace arc.common.Models.Coding;
public class ConditionListModel
{
    public int Id { get; set; }
    public int AntibioticId { get; set; }
    public int TestMethodId { get; set; }
    public decimal Measurement { get; set; }
    public int SusceptibilityId { get; set; }
}
