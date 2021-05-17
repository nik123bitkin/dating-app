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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly ILikeRepository _likeRepo;

        public UserService(IUserRepository userRepo, IMapper mapper, ILikeRepository likeRepo)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _likeRepo = likeRepo;
        }

        public async Task<(PagedList<User>, IEnumerable<UserForListDto>)> GetUsersAsync(int id, UserParams userParams)
        {
            var userFromRepo = await _userRepo.GetByIdAsync(id);

            userParams.UserId = id;

            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = userFromRepo.Gender == "male" ? "female" : "male";
            }

            var users = await _userRepo.GetUsersAsync(userParams);

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            return (users, usersToReturn);
        }

        public async Task UpdateUserAsync(int id, UserForUpdateDto userForUpdateDto)
        {
            var userFromRepo = await _userRepo.GetByIdAsync(id);

            _mapper.Map(userForUpdateDto, userFromRepo);

            await _userRepo.SaveAllAsync();
        }

        public async Task<UserForDetailedDto> GetUserAsync(int id)
        {
            var user = await _userRepo.GetUserAsync(id);

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);

            return userToReturn;
        }

        public async Task LikeUserAsync(int id, int recipientId)
        {
            var like = await _userRepo.GetLikeAsync(id, recipientId);

            if (like != null)
            {
                throw new AlreadyExistsException();
            }

            var user = await _userRepo.GetByIdAsync(recipientId);
            if (user == null)
            {
                throw new NotFoundException();
            }

            like = new Like
            {
                LikerId = id,
                LikeeId = recipientId
            };

            _likeRepo.Add(like);

            await _userRepo.SaveAllAsync();
        }

        public async Task LogActivityAsync(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);

            user.LastActive = DateTime.Now;

            await _userRepo.SaveAllAsync();
        }
    }
}
