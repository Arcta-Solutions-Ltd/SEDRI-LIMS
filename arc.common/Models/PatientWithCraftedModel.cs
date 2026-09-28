using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arc.common.Models
{
    public class PatientWithCraftedModel
    {
        public string PatientId { get; set; }
        public string PatientRef { get; set; }
        /// <summary>
        /// Patient date of birth (ISO format) for specimen age auto-calculation when creating specimen from patient list.
        /// </summary>
        public string DateOfBirth { get; set; }
        public List<CraftedModel> Crafted { get; set; }
    }
}
