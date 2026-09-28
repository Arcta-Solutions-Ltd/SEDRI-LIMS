using System;
using System.Collections.Generic;

namespace arc.domain.Quality
{
    public class IqcTestProfile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TestMethodListItemId { get; set; }
        public List<IqcTestProfileQcOrganism> IqcTestProfileQcOrganisms { get; set; } = new List<IqcTestProfileQcOrganism>();
        public enum TestMethod
        {
            Disk,
            MIC
        }
        public DateTime LastModifiedDate { get; set; }
        public DateTime DeletedDate { get; set; }

        public IqcTestProfile(string name, int testMethodListItemId)
        {
            Name = name;
            TestMethodListItemId = testMethodListItemId;
        }

        public IqcTestProfile() { }

        public static string GetTestMethodAsString(TestMethod testMethod)
        {
            return testMethod switch
            {
                TestMethod.Disk => "Disk",
                TestMethod.MIC => "MIC",
                _ => throw new Exception("Invalid Test Method"),
            };
        }

        public static string[] TestMethods()
        {
            return Enum.GetNames(typeof(TestMethod));
        }
    }
}
