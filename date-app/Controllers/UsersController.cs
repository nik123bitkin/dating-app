using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AppCore.DTOs;
using AppCore.Entities;
using AppCore.Exceptions;
using AppCore.HelperEntities;
using AppCore.Interfaces;
using date_app.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

            (PagedList<User> usersForPaging, IEnumerable<UserForListDto> usersToReturn) = await _userService.GetUsers(currentUserId, userParams);
            var pagination = new Pagination(usersForPaging.CurrentPage, usersForPaging.PageSize, usersForPaging.TotalCount, usersForPaging.TotalPages);
            var paginatedResponse = new PaginatedResponse<UserForListDto>(usersToReturn, pagination);

            return Ok(paginatedResponse);
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
            catch
            {
                return Problem("Internal server error.");
            }
        }
    }
}
