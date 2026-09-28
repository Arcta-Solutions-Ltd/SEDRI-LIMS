using arc.app.Security;
using arc.data.Security;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace arc.data.Configuration
{
    public class SqlSetupRepository : ISetupRespository
    {
        private readonly IOptionsMonitor<DataOptions> _options;
        private readonly ILogger _logger;

        public SqlSetupRepository(IOptionsMonitor<DataOptions> options, ILogger logger)
        {
            _options = options;
            _logger = logger;
        }

        public async Task SaveSetup(string message)
        {
            try
            {
                //var currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                var systemAdminUserRole = new UserRoleDataModel();
                var orgAdminUserRole = new UserRoleDataModel();

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
                    {
                        // Check whether config record exists and add if not

                        var sql = "";
                        var configData = JsonConvert.DeserializeObject<ConfigDataModel>(message);

                        // Add system admin role

                        //if (configData.MultipleUserFlag == "Yes")
                        //{
                        //    var menuPermission = @"{ 'AllowedSidebarItems' : ['admin', 'users', 'roles'] }";
                        //    var eventPermission = @"{ 'AllowedEvents' : ['adduser','edituser','addrole','clonerole','deleterole','editrole','menupermissions','eventpermissions'] }";
                        //    menuPermission = menuPermission.Replace('\'', '"');
                        //    eventPermission = eventPermission.Replace('\'', '"');
                        //    sql = @"insert into Role(RoleName, LastModifiedDate, MenuPermission, EventPermission, RoleDescription, Enabled) 
                        //            values('SystemAdmin', now(),'" + menuPermission + "','" + eventPermission + @"', 'Default System Administrator', 'Yes') 
                        //            returning Id";
                        //    systemAdminUserRole.RoleId = connect.Query<int>(sql).Single();
                        //}

                        systemAdminUserRole.RoleId = 1;
                        orgAdminUserRole.RoleId = 2;

                        var userData = JsonConvert.DeserializeObject<SystemAdminUserDataModel>(message);
                        userData.Enabled = "Yes";
                        sql = @"insert into Users(Username, FirstName, LastName, Password, Enabled, LastModifiedDate) 
                                values(@SystemAdminUsername,@SystemAdminFirstName,@SystemAdminLastName,@SystemAdminPassword, @Enabled, now()) 
                                returning Id";
                        systemAdminUserRole.UserId = connect.Query<int>(sql, userData).Single();

                        sql = @"insert into UserRole(UserId, RoleId, LastModifiedDate)
                                values(@UserId, @RoleId, now())";
                        await connect.ExecuteAsync(sql, systemAdminUserRole);

                        // Add the org admin user

                        var orgUserData = JsonConvert.DeserializeObject<OrganisationAdminUserDataModel>(message);
                        orgUserData.Enabled = "Yes";
                        sql = @"insert into Users(Username, FirstName, LastName, Password, Enabled, LastModifiedDate) 
                                values(@OrgAdminUsername,@OrgAdminFirstName,@OrgAdminLastName,@OrgAdminPassword, @Enabled, now()) 
                                returning Id";
                        orgAdminUserRole.UserId = connect.Query<int>(sql, orgUserData).Single();

                        sql = @"insert into UserRole(UserId, RoleId, LastModifiedDate)
                                values(@UserId, @RoleId, now())";
                        await connect.ExecuteAsync(sql, orgAdminUserRole);

                        // Add the default laboratory

                        var labId = 1;

                        sql = @"insert into LaboratoryUser(UserId, LaboratoryId)
                                    values(@UserId, @labId)";
                        await connect.ExecuteAsync(sql, new { orgAdminUserRole.UserId, labId });

                        if (systemAdminUserRole.UserId > 0)
                        {
                            sql = @"insert into LaboratoryUser(UserId, LaboratoryId)
                                    values(@UserId, @labId)";
                            await connect.ExecuteAsync(sql, new { systemAdminUserRole.UserId, labId });
                        }
                    }

                    scope.Complete();
                }
            }
            catch (TransactionAbortedException ex)
            {
                _logger.LogError("Create Event Transaction aborted : {0}", ex.Message);
            }
            catch (Exception e)
            {
                _logger.LogError("Create Event execution error : {0}", e.Message);
            }

        }
    }
}

