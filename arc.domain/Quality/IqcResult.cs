using System;

namespace arc.domain.Quality
{
    public class IqcResult
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public int QcAntibioticId { get; set; }
        public decimal? Value { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
