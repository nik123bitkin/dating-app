using System.Collections.Generic;
using System.Threading.Tasks;
using AppCore.DTOs;
using AppCore.Entities;
using AppCore.HelperEntities;

namespace AppCore.Interfaces
{
    public interface IMessageService
    {
        public Task<Message> GetMessageAsync(int userId, int id);

        public Task<MessageToReturnDto> CreateMessageAsync(int userId, MessageForCreationDto messageForCreationDto);

        public Task<(IEnumerable<MessageToReturnDto>, PagedList<Message>)> GetMessagesForUserAsync(int userId, MessageParams messageParams);

        public Task<IEnumerable<MessageToReturnDto>> GetMessageThreadAsync(int userId, int recipientId);

        public Task DeleteMessageAsync(int id, int userId);

        public Task MarkMessageAsReadAsync(int userId, int id);
    }
}
