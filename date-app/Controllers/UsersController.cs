using AutoMapper;
using AppCore.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AppCore.Interfaces;
using AppCore.Entities;
using AppCore.Helpers;
using AppCore.HelperEntities;

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

            var updateResult = await _userService.UpdateUser(id, userForUpdateDto);

            if (updateResult == ReturnTypes.Good)
            {
                return NoContent();
            }
            else
            {
                return BadRequest($"Updating user with {id} failed on save");
            }
        }

        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int id, int recipientId)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var result = await _userService.LikeUser(id, recipientId);

            switch (result)
            {
                case ReturnTypes.SaveError:
                    return BadRequest("Failed to like user");
                case ReturnTypes.DataError:
                    return BadRequest("You have already liked this user");
                case ReturnTypes.NotFound:
                    return NotFound();
                default:
                    return Ok();
            }
        }
    }
}
