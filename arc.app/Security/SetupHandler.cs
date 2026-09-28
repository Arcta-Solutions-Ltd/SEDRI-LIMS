using arc.app.Common;
using arc.app.Config.Queries;
using arc.app.Config.Security;
using arc.common.Utils;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Security.User;
using arc.identity;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace arc.app.Security
{
    public class SetupHandler : ISetupHandler
    {
        private string _validationMessage = "";
        private readonly IHandleQuery _queryHandler;
        private readonly ISetupRespository _repository;
        private readonly IJsonMapper _jsonMapper;
        private readonly IPasswordUtils _passwordUtils;
        private readonly IJsonUtils _jsonUtils;
        private readonly IQueryFactory _queryFactory;


        public SetupHandler(IHandleQuery queryHandler, ISetupRespository repository, IJsonMapper jsonMapper, IPasswordUtils passwordUtils,
                            IJsonUtils jsonUtils, IQueryFactory queryFactory)
        {
            _queryHandler = queryHandler;
            _queryFactory = queryFactory;
            _repository = repository;
            _jsonMapper = jsonMapper;
            _passwordUtils = passwordUtils;
            _jsonUtils = jsonUtils;
        }

        public async Task Handle(string message)
        {
            // Get config data from event

            var configJson = new SetupEvent().GetConfig();
            var eventConfig = JsonConvert.DeserializeObject<EventConfig>(configJson);

            // Compare passwords

            if (_jsonUtils.GetSingleFieldValue(message,"SystemAdminPassword") != _jsonUtils.GetSingleFieldValue(message, "SystemAdminConfirmPassword")) {
                _validationMessage = "System admin passwords must be the same";
            }

            if (_jsonUtils.GetSingleFieldValue(message, "OrgAdminPassword") != _jsonUtils.GetSingleFieldValue(message, "OrgAdminConfirmPassword")) {
                _validationMessage = "Organisation admin passwords must be the same";
            }

            if (!string.IsNullOrEmpty(_validationMessage)) { return; }

            // Hash passwords

            var systemAdminMapping = @"[{Type: 'Mapping', Source: 'SystemAdminUserName', Target: 'Username'},
                                        {Type: 'Mapping', Source: 'SystemAdminFirstName', Target: 'FirstName'}, 
                                        {Type: 'Mapping', Source: 'SystemAdminLastName', Target: 'LastName'}, 
                                        {Type: 'Mapping', Source: 'SystemAdminPassword', Target: 'Password'},
                                        {Type: 'New', Target: 'Enabled', Value: 'Yes'}]";
            var transformedMessage = _jsonMapper.Transform(message, systemAdminMapping);
            var newUser = JsonConvert.DeserializeObject<User>(transformedMessage);

            if (! string.IsNullOrEmpty(newUser.Password))
            {
                var newPassword = _passwordUtils.GetPassword(newUser);
                var newPasswordMapping = @"[{Type: 'Mapping', Source: 'SystemAdminPassword', Target: 'SystemAdminPassword', Value: '" + newPassword + "'}," +
                                            "{Type: 'Mapping', Source: 'SystemAdminConfirmPassword', Target: 'SystemAdminConfirmPassword', Value: '" + newPassword + "'}]";
                message = _jsonMapper.Transform(message, newPasswordMapping, true);
            }


            var orgAdminMapping = @"[{Type: 'Mapping', Source: 'OrgAdminUserName', Target: 'Username'},
                                    {Type: 'Mapping', Source: 'OrgAdminFirstName', Target: 'FirstName'}, 
                                    {Type: 'Mapping', Source: 'OrgAdminLastName', Target: 'LastName'}, 
                                    {Type: 'Mapping', Source: 'OrgAdminPassword', Target: 'Password'},
                                    {Type: 'New', Target: 'Enabled', Value: 'Yes'}]";
            transformedMessage = _jsonMapper.Transform(message, orgAdminMapping);
            newUser = JsonConvert.DeserializeObject<User>(transformedMessage);

            if (! string.IsNullOrEmpty(newUser.Password))
            {
                var newPassword = _passwordUtils.GetPassword(newUser);
                var newPasswordMapping = @"[{Type: 'Mapping', Source: 'OrgAdminPassword', Target: 'OrgAdminPassword', Value: '" + newPassword + "'}, " +
                                        "{Type: 'Mapping', Source: 'OrgAdminConfirmPassword', Target: 'OrgAdminConfirmPassword', Value: '" + newPassword + "'}]";
                message = _jsonMapper.Transform(message, newPasswordMapping, true);
            }

            // Validate the data

            _validationMessage = eventConfig.ValidateMessage(message);
            if (! string.IsNullOrEmpty(_validationMessage)) { return; }

            // Check the system is empty

            var parameters = "{ Name: 'UserCount'}";
            var queryFilters = JsonConvert.DeserializeObject<QueryFilterConfig>(parameters);
            var queryData = _queryFactory.GetQuery(queryFilters.Name);
            var recordCount = await _queryHandler.HandleAsync(parameters, queryFilters, queryData);

//            var recordCount = await _queryHandler.Handle("{ Name: 'UserCount'}");

            if (recordCount != "0") {
                _validationMessage = "System is not in an empty state so the initial configuration cannot be carried out";
                return; 
            }

            // Save the data

            await _repository.SaveSetup(message);
        }

        public string GetValidationMessage()
        {
            return _validationMessage;
        }
    }

}
