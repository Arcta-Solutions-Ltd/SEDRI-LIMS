using System.Collections.Generic;

namespace arc.common.Models.QualityAssurance
{
    public class IqcTestModel
    {
        public List<ContentsCraftedModel> Crafted { get; set; }
        public string TestMethodId { get; set; }
        public int Id { get; set; }

        public IqcTestModel() { }

        public IqcTestModel(IqcTestModel addIqcTestModel)
        {
            Crafted = addIqcTestModel.Crafted;
            TestMethodId = addIqcTestModel.TestMethodId;
        }
    }
}
