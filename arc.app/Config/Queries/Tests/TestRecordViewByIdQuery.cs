using arc.app.Common;

namespace arc.app.Config.Queries.Tests;

/// <summary>
/// Query configuration for TestRecordViewById. Returns Id, TestName, TestResults, Status
/// for either a culture test or direct test based on the source parameter.
/// </summary>
internal class TestRecordViewByIdQuery : IDefinition
{
    public string Get()
    {
        return @"{
            'Query': 'testrecordviewbyid',
            'TableName': 'CultureTests',
            'Type': 'Special'
        }";
    }
}
