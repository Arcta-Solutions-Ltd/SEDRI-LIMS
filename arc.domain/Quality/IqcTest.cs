using System;
using System.Collections.Generic;

namespace arc.domain.Quality
{
    public class IqcTest
    {
        public int Id { get; set; }
        public int StateId { get; set; }
        public int IqcTestProfileId { get; set; }
        public string Comments { get; set; }
        public List<IqcResult> Results { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CompletedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
