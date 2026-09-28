using System;
using System.Collections.Generic;

namespace arc.domain.Quality
{
    public class QcOrganism
    {
        public int Id { get; set; }
        public int OrgamismId { get; set; }
        public bool UseByDefault { get; set; }
        public bool Enabled { get; set; }
        public string StandardsBody { get; set; }
        public string PrimaryStrain { get; set; }
        public string OtherStrains { get; set; }
        public List<QcAntibiotic> QcAntibiotics { get; set; } = new List<QcAntibiotic>();
        public DateTime LastModifiedDate { get; set; }
    }
}
