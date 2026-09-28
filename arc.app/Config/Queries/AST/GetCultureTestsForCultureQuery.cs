using arc.app.Common;

namespace arc.app.Config.Queries.AST
{
    /// <summary>
    /// Query configuration for GetCultureTestsForCulture. Returns only CultureTests for a culture;
    /// used when refetching after an isolate test save so the AST panel turns green.
    /// </summary>
    internal class GetCultureTestsForCultureQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'getculturetestsforculture', 'Type': 'Special', 'TableName': 'CultureTests' }";
        }
    }
}
