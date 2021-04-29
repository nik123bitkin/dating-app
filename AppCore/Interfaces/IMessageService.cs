using System.Collections.Generic;
using System.Threading.Tasks;
using AppCore.DTOs;
using AppCore.Entities;
using AppCore.HelperEntities;

namespace AppCore.Interfaces
{
    public interface IMessageService
    {
        public Task<Message> GetMessage(int userId, int id);

        public Task<MessageToReturnDto> CreateMessage(int userId, MessageForCreationDto messageForCreationDto);

        public Task<(IEnumerable<MessageToReturnDto>, PagedList<Message>)> GetMessagesForUser(int userId, MessageParams messageParams);

        public Task<IEnumerable<MessageToReturnDto>> GetMessageThread(int userId, int recipientId);

        public Task DeleteMessage(int id, int userId);

        public Task MarkMessageAsRead(int userId, int id);
    }
}
