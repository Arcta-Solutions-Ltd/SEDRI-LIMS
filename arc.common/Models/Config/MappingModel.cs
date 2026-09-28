using System.Collections.Generic;

namespace arc.common.Models.Config;

public class MappingListViewModel
{
    public int Id { get; set; }
    public string ConfigName { get; set; }
    public string Name { get; set; }

}

public class MappingModel
{
    public string Name { get; set; }
    public List<MappingValuesModel> Mapping { get; set; }

}

public class MappingValuesModel
{
    public string BeforeMappingValue { get; set; }
    public string AfterMappingValue { get; set; }
}
