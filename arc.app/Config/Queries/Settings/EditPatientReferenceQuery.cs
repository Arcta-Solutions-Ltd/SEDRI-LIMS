using arc.app.Common;

namespace arc.app.Config.Queries;
internal class EditPatientReferenceQuery : IDefinition
{
    public string Get()
    {
        return @"{ 'Query': 'editpatientreferencequery', 'Type': 'Config', Tablename: 'Configs', Translate: true}";
    }
}
