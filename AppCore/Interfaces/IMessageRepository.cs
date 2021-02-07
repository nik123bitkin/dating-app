using AppCore.Entities;
using AppCore.HelperEntities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppCore.Interfaces
{
    public interface IMessageRepository : IDataRepository
    {
        Task<Message> GetMessage(int id);
        Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId);
    }
}
