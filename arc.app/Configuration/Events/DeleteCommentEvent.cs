using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using System;
using System.Threading.Tasks;


namespace arc.app.Configuration.Events
{
    internal class DeleteCommentEvent : IRun
    {
        private readonly ICommentRepository _commentRepository;

        public DeleteCommentEvent(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            await _commentRepository.DeleteCommentAsync(id);
            return 0;
        }
    }
}

