namespace arc.common.Models.QualityAssurance
{
    public class AddIqcResultsToIqcTestModel : IqcTestModel
    {
        public int IqcTestId { get; set; }

        public AddIqcResultsToIqcTestModel() { }

        public AddIqcResultsToIqcTestModel(int iqcTestId, IqcTestModel addIqcTestModel) : base(addIqcTestModel)
        {
            IqcTestId = iqcTestId;
        }
    }
}
