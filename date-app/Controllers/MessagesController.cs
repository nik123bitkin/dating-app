using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AppCore.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AppCore.Entities;
using AppCore.HelperEntities;
using date_app.Helpers;
using Infrastructure.Interfaces;
using AppCore.Interfaces;
using AppCore.Exceptions;

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

            try
            {
                var message = _messageService.GetMessage(userId, id);
                return Ok(message);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch
            {
                return Problem("Internal server error.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, MessageForCreationDto messageForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            try
            {
                var message = await _messageService.CreateMessage(userId, messageForCreationDto);
                return CreatedAtRoute("GetMessage", new { userId, id = message.Id }, message);
            }
            catch (NotFoundException)
            {
                return NotFound("User not found.");
            }
            catch (SaveDataException)
            {
                return Problem("Error occured during saving process.");
            }
            catch
            {
                return Problem("Internal server error.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser(int userId, [FromQuery]MessageParams messageParams)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }    
            try
            {
                (IEnumerable<MessageToReturnDto> messages, PagedList<Message> messagesFromRepo) 
                    = await _messageService.GetMessagesForUser(userId, messageParams);

                Response.AddPagination(messagesFromRepo.CurrentPage, messagesFromRepo.PageSize,
                    messagesFromRepo.TotalCount, messagesFromRepo.TotalPages);

                return Ok(messages);
            }
            catch
            {
                return Problem("Internal server error.");
            }
        }

        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(int userId, int recipientId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            try
            {
                var thread = await _messageService.GetMessageThread(userId, recipientId);
                return Ok(thread);
            }
            catch
            {
                return Problem("Internal server error.");
            }
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage(int id, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            try
            {
                await _messageService.DeleteMessage(id, userId);
                return NoContent();
            }
            catch (SaveDataException)
            {
                return Problem("Error occured during saving process.");
            }
            catch
            {
                return Problem("Internal server error.");
            }
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            try
            {
                await _messageService.MarkMessageAsRead(userId, id);
                return NoContent();

            }
            catch (NotFoundException)
            {
                return NotFound("Failed to make message as read. Invalid recipient.");
            }
            catch (SaveDataException)
            {
                return Problem("Error occured during saving process.");
            }
            catch
            {
                return Problem("Internal server error.");
            }
        }
    }
}
