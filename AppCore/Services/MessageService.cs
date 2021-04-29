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

        public async Task<MessageToReturnDto> CreateMessage(int userId, MessageForCreationDto messageForCreationDto)
        {
            messageForCreationDto.SenderId = userId;

            var recipient = await _userRepo.GetUser(messageForCreationDto.RecipientId);
            if (recipient == null)
            {
                throw new NotFoundException();
            }

            var message = _mapper.Map<Message>(messageForCreationDto);

            _messageRepo.Add(message);

            try
            {
                await _messageRepo.SaveAll();
                var messageToReturn = _mapper.Map<MessageToReturnDto>(message);
                return messageToReturn;
            }
            catch
            {
                throw;
            }
        }

        public async Task DeleteMessage(int id, int userId)
        {
            var messageFromRepo = await _messageRepo.GetById(id);
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

            try
            {
                await _messageRepo.SaveAll();
            }
            catch
            {
                throw;
            }
        }

        public async Task<Message> GetMessage(int userId, int id)
        {
            var messageFromRepo = await _messageRepo.GetById(id);
            if (messageFromRepo == null)
            {
                throw new NotFoundException();
            }

            return messageFromRepo;
        }

        public async Task<(IEnumerable<MessageToReturnDto>, PagedList<Message>)> GetMessagesForUser(int userId, MessageParams messageParams)
        {
            messageParams.UserId = userId;

            var messagesFromRepo = await _messageRepo.GetMessagesForUser(messageParams);

            var messages = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);

            return (messages, messagesFromRepo);
        }

        public async Task<IEnumerable<MessageToReturnDto>> GetMessageThread(int userId, int recipientId)
        {
            var messagesFromRepo = await _messageRepo.GetMessageThread(userId, recipientId);

            var messageThread = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);

            return messageThread;
        }

        public async Task MarkMessageAsRead(int userId, int id)
        {
            var message = await _messageRepo.GetById(id);
            if (message.RecipientId != userId)
            {
                throw new NotFoundException();
            }

            message.IsRead = true;
            message.DateRead = DateTime.Now;

            try
            {
                await _messageRepo.SaveAll();
            }
            catch
            {
                throw;
            }
        }
    }
}
