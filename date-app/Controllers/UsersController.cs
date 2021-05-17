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

            (PagedList<User> usersForPaging, IEnumerable<UserForListDto> usersToReturn) = await _userService.GetUsersAsync(currentUserId, userParams);
            var pagination = new Pagination(usersForPaging.CurrentPage, usersForPaging.PageSize, usersForPaging.TotalCount, usersForPaging.TotalPages);
            var paginatedResponse = new PaginatedResponse<UserForListDto>(usersToReturn, pagination);

            return Ok(paginatedResponse);
        }

        [HttpGet("{id}", Name ="GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserAsync(id);

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            await _userService.UpdateUserAsync(id, userForUpdateDto);
            return NoContent();
        }

        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int id, int recipientId)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            await _userService.LikeUserAsync(id, recipientId);
            return Ok();
        }
    }
}
