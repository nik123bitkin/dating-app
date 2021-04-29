using System.Collections.Generic;
using System.Threading.Tasks;
using AppCore.Entities;
using AppCore.HelperEntities;

namespace AppCore.Interfaces
{
    public interface IMessageRepository : IDataRepository<Message>
    {
        Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams);

        Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId);
    }
}
