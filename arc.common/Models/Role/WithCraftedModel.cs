using System.Collections.Generic;

namespace arc.common.Models.Role
{
    public class WithCraftedModel<T>
    {
        public List<T> Crafted { get; set; }
    }
}
