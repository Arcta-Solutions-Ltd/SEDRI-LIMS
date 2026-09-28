using arc.common.Models.Role;
using System.Collections.Generic;

namespace arc.common.Models
{
    public class CraftedWithIdForEventModel<T> : WithCraftedModel<T>
    {
        public string Id { get; set; }
        public string Event { get; set; }
    }
}
