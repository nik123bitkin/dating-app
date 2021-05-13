using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppCore.DTOs;
using AppCore.Entities;
using AppCore.Exceptions;
using AppCore.HelperEntities;
using AppCore.Interfaces;
using AutoMapper;

namespace AppCore.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public MessageService(IMessageRepository messageRepo, IUserRepository userRepo, IMapper mapper)
        {
            _messageRepo = messageRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<MessageToReturnDto> CreateMessageAsync(MessageForCreationDto messageForCreationDto)
        {
            var recipient = await _userRepo.GetByIdAsync(messageForCreationDto.RecipientId);
            if (recipient == null)
            {
                throw new NotFoundException("Invalid recepient. User with such ID not found.");
            }

            var message = _mapper.Map<Message>(messageForCreationDto);

            _messageRepo.Add(message);

            await _messageRepo.SaveAllAsync();
            var messageToReturn = _mapper.Map<MessageToReturnDto>(message);
            return messageToReturn;
        }

        public async Task DeleteMessageAsync(int id, int userId)
        {
            var messageFromRepo = await _messageRepo.GetByIdAsync(id);
            if (messageFromRepo.SenderId == userId)
            {
                messageFromRepo.SenderDeleted = true;
            }

            if (messageFromRepo.RecipientId == userId)
            {
                messageFromRepo.RecipientDeleted = true;
            }

            if (messageFromRepo.SenderDeleted && messageFromRepo.RecipientDeleted)
            {
                _messageRepo.Delete(messageFromRepo);
            }

            await _messageRepo.SaveAllAsync();
        }

        public async Task<Message> GetMessageAsync(int id)
        {
            var messageFromRepo = await _messageRepo.GetByIdAsync(id);
            if (messageFromRepo == null)
            {
                throw new NotFoundException("Message not found.");
            }

            return messageFromRepo;
        }

        public async Task<(IEnumerable<MessageToReturnDto>, PagedList<Message>)> GetMessagesForUserAsync(MessageParams messageParams)
        {
            var messagesFromRepo = await _messageRepo.GetMessagesForUserAsync(messageParams);

            var messages = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);

            return (messages, messagesFromRepo);
        }

        public async Task<IEnumerable<MessageToReturnDto>> GetMessageThreadAsync(int userId, int recipientId)
        {
            var messagesFromRepo = await _messageRepo.GetMessageThreadAsync(userId, recipientId);

            var messageThread = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);

            return messageThread;
        }

        public async Task MarkMessageAsReadAsync(int userId, int id)
        {
            var message = await _messageRepo.GetByIdAsync(id);
            if (message.RecipientId != userId)
            {
                throw new NotFoundException("Invalid recepient. User with such ID not found.");
            }

            message.IsRead = true;
            message.DateRead = DateTime.Now;

            await _messageRepo.SaveAllAsync();
        }
    }
}
