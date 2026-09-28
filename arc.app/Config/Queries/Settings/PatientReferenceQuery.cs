using arc.app.Common;

namespace arc.app.Config.Queries;
internal class PatientReferenceQuery : IDefinition
{
    public string Get()
    {
        return @"{ 'Query': 'patientreferencequery', 'Type': 'Config', Tablename: 'Configs', Translate: true}";
    }
}
