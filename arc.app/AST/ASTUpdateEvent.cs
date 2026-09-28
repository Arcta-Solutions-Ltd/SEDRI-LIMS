using arc.app.Common;
using arc.common;
using arc.common.Models;
using arc.domain.Configuration.EventsConfig;
using System.Threading.Tasks;

namespace arc.app.AST
{
    public class ASTUpdateEvent : IRun
    {
        public IASTRepository _ASTRepository;
        public TokenInfoModel _token;

        public ASTUpdateEvent(IASTRepository ASTRepository, TokenInfoModel token)
        {
            _ASTRepository = ASTRepository;
            _token = token;
        }

        public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
        {
            string username = _token.Username;
            return await _ASTRepository.UpdateASTAsync(dataToSave, username);
        }

    }
}
