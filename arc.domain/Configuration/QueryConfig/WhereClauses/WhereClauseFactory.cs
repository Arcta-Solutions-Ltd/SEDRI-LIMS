using arc.common.Models;

namespace arc.domain.Configuration.QueryConfig.WhereClauses
{
    internal class WhereClauseFactory
    {
        private readonly TokenInfoModel _token;

        internal WhereClauseFactory(TokenInfoModel token)
        {
            _token = token;
        }

        internal IWhereClause Create(string name)
        {
            return name switch
            {
                "=" => new EqualToClause(),
                "childexists" => new ChildExistsClause(_token),
                "childexistsornull" => new ChildExistsOrNullClause(_token),
                "agerange" => new AgeRangeClause(),
                "daterange" => new DateRangeClause(),
                "dateofbirth" => new DateOfBirthClause(),
                "equals" => new EqualsClause(),
                "startswith" => new StartsWithClause(),
                "contains" => new ContainsClause(),
                "oneof" => new OneOfClause(),
                "dayselapsed" => new DaysElapsedClause(),
                "multipleoneof" => new MultipleOneOfClause(),
                "in" => new InClause(),
                "withinlast" => new WithinLastClause(),
                _ => new EqualToClause(),
            };
        }
    }
}
