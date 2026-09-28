using arc.app.Roles;
using arc.data.Configuration;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Security.Permission;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

namespace arc.data.Security;

public class RoleRepository : IRoleRepository
{
    private readonly IOptionsMonitor<DataOptions> _options;
    private readonly ILogger _logger;

    public RoleRepository(IOptionsMonitor<DataOptions> options, ILogger logger)
    {
        _options = options;
        _logger = logger;
    }

    public async Task<IEnumerable<Role>> GetRolesForUserAsync(string username)
    {
        using var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection);
        var sql = """
            select
                *
            from
                Role r
                inner join UserRole ur on ur.RoleId = r.Id
                inner join Users u on ur.UserId = u.Id
            where
                u.Username = @username
            """;

        var result = await connect.QueryAsync<Role>(sql, new { username });

        return result;
    }

    public async Task<Role> GetRoleByIdAsync(int roleId)
    {
        using var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection);

        var sql = "select * from Role where id = @roleId";

        return await connect.QueryFirstOrDefaultAsync<Role>(sql, new { roleId });
    }

    public async Task<IEnumerable<OptionsConfig>> GetRolesForListAsync()
    {
        using var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection);

        var sql = @"select id as key, rolename As text from role order by rolename";

        return await connect.QueryAsync<OptionsConfig>(sql);
    }

    public async Task DeleteRoleAsync(int roleId)
    {
        try
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
            {
                var sql = @"delete from userrole where roleid = @roleId";
                await connect.ExecuteAsync(sql, new { roleId });

                sql = @"delete from role where id = @roleId";
                await connect.ExecuteAsync(sql, new { roleId });
            }

            scope.Complete();
        }
        catch (TransactionAbortedException ex)
        {
            _logger.LogError("Delete Role Transaction aborted : {exceptionMessage}", ex.Message);
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError("Delete Role sql execution error : {exceptionMessage}", e.Message);
            throw;
        }
    }

    public async Task<int> CloneRoleAsync(int roleId, string roleName, string roleDescription)
    {
        try
        {
            using var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection);

            var sql = """
                insert into
                    Role (
                        rolename,
                        roledescription,
                        lastmodifieddate,
                        enabled,
                        moredata,
                        menupermission,
                        eventpermission
                    )
                select
                    @roleName,
                    @roleDescription,
                    now(),
                    enabled,
                    moredata,
                    menupermission,
                    eventpermission
                from
                    Role
                where
                    id = @roleId returning id
                """;

            return await connect.QueryFirst(sql, new { roleId, roleName, roleDescription });
        }
        catch (Exception e)
        {
            _logger.LogError("Clone Role sql execution error : {exceptionMessage}", e.Message);
            return 0;
        }
    }
}
