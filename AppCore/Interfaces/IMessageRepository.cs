using System.Collections.Generic;
using System.Threading.Tasks;
using AppCore.Entities;
using AppCore.HelperEntities;

namespace AppCore.Interfaces
{
    public interface IMessageRepository : IDataRepository<Message>
    {
        Task<PagedList<Message>> GetMessagesForUserAsync(MessageParams messageParams);

        Task<IEnumerable<Message>> GetMessageThreadAsync(int userId, int recipientId);
    }
}
