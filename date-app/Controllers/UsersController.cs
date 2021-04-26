using AppCore.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AppCore.Interfaces;
using AppCore.Entities;
using AppCore.HelperEntities;
using date_app.Helpers;
using AppCore.Exceptions;

namespace date_app.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            (PagedList<User> usersForPaging, IEnumerable<UserForListDto> usersToReturn) returnedUsers = await _userService.GetUsers(currentUserId, userParams);

            Response.AddPagination(returnedUsers.usersForPaging.CurrentPage, returnedUsers.usersForPaging.PageSize, 
                returnedUsers.usersForPaging.TotalCount, returnedUsers.usersForPaging.TotalPages);

            return Ok(returnedUsers.usersToReturn);
        }

        [HttpGet("{id}", Name ="GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUser(id);

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            try
            {
                await _userService.UpdateUser(id, userForUpdateDto);
                return NoContent();
            }
            catch (SaveDataException)
            {
                return Problem($"Updating user with {id} failed on save");
            }
            catch
            {
                return Problem("Internal server error.");
            }
        }

        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int id, int recipientId)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            try
            {
                await _userService.LikeUser(id, recipientId);
                return Ok();
            }
            catch (AlreadyExistsException)
            {
                return Ok("You have already liked this user");
            }
            catch (NotFoundException)
            {
                return NotFound("Failed to like user. Recipient not found");
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
