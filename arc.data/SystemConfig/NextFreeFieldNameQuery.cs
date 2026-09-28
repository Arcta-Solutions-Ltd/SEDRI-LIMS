using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.SystemConfig
{
    internal class NextFreeFieldNameQuery : IQueryReturningString
    {
        private int _length = 0;

        public async Task<string> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var name = queryFilters.Parameters.Where(p => p.Key.ToLower() == "name").First();
            var currentNumber = GetNumberAtEndOfString(name.Value);
            var returnName = name.Value.Substring(0, name.Value.Length - _length);

            var sql = @"select count(*) from namelist where Lower(Name) = Lower(@Name)";
            var result = connect.QueryFirst<long>(sql, new { Name = returnName.ToLower() + currentNumber.ToString() });

            while (result > 0)
            {
                currentNumber++;

                sql = @"select count(*) from namelist where Lower(Name) = Lower(@Name)";
                result = connect.QueryFirst<long>(sql, new { Name = returnName.ToLower() + currentNumber.ToString() });
            };

            sql = @"insert into NameList(Name) Values(@Name)";
            await connect.ExecuteAsync(sql, new { Name = returnName.ToLower() + currentNumber.ToString() });

            return returnName + currentNumber.ToString();
        }


        private int GetNumberAtEndOfString(string value)
        {
            var stack = new Stack<char>();
            _length = 0;

            for (var i = value.Length - 1; i >= 0; i--)
            {
                if (!char.IsNumber(value[i]))
                {
                    break;
                }

                _length++;
                stack.Push(value[i]);
            }

            var result = new string(stack.ToArray());

            if (result == "") { return 0; } else { return int.Parse(result); }
        }
    }
}
