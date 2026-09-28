using arc.common;
using arc.common.ExtensionMethods;
using arc.common.Models.Coding;
using arc.domain.Coding;

namespace arc.data.Coding.TestPatterns
{
    /// <summary>
    /// Maps the TestPatternModel type onto the TestPattern type.
    /// </summary>
    public class TestPatternModelToTestPatternMapper : IMapType<TestPatternModel, TestPattern>
    {
        /// <summary>
        /// Maps a <see cref="TestPatternModel"/> object to a <see cref="TestPattern"/> object.
        /// </summary>
        /// <param name="testPatternModel">The <see cref="TestPatternModel"/> object to map.</param>
        /// <returns>The mapped <see cref="TestPattern"/> object.</returns>
        public TestPattern Map(TestPatternModel testPatternModel)
        {
            var testPattern = testPatternModel.Map<TestPattern>();

            testPattern.AntibioticGrid = [];

            foreach (var line in testPatternModel.AntibioticGrid)
            {
                var testPatternLine = new TestPatternLine
                {
                    Id = line.Id,
                    testOrder = line.testOrder,
                    AntibioticId = line.AntibioticId,
                    Dosage = line.Dosage,
                    TestMethodId = line.TestMethodId,
                    GuidelinesId = line.GuidelinesId,
                    CategoryId = line.CategoryId,
                    PrintOnReport = line.PrintOnReport == "Yes" ? true : false
                };

                testPattern.AntibioticGrid.Add(testPatternLine);
            }

            return testPattern;
        }
    }
}
