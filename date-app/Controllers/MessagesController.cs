using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AppCore.DTOs;
using AppCore.Entities;
using AppCore.HelperEntities;
using AppCore.Interfaces;
using date_app.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace date_app.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var message = await _messageService.GetMessageAsync(id);
            return Ok(message);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, MessageForCreationDto messageForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            messageForCreationDto.SenderId = userId;
            var message = await _messageService.CreateMessageAsync(messageForCreationDto);
            return CreatedAtRoute("GetMessage", new { userId, id = message.Id }, message);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser(int userId, [FromQuery]MessageParams messageParams)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            messageParams.UserId = userId;
            (IEnumerable<MessageToReturnDto> messages, PagedList<Message> messagesFromRepo)
                = await _messageService.GetMessagesForUserAsync(messageParams);

            var pagination = new Pagination(messagesFromRepo.CurrentPage, messagesFromRepo.PageSize, messagesFromRepo.TotalCount, messagesFromRepo.TotalPages);
            var paginatedResponse = new PaginatedResponse<MessageToReturnDto>(messages, pagination);
            return Ok(paginatedResponse);
        }

        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(int userId, int recipientId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var thread = await _messageService.GetMessageThreadAsync(userId, recipientId);
            return Ok(thread);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage(int id, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            await _messageService.DeleteMessageAsync(id, userId);
            return NoContent();
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            await _messageService.MarkMessageAsReadAsync(userId, id);
            return NoContent();
        }
    }
}
