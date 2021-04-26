using AppCore.DTOs;
using AppCore.Entities;
using AppCore.Exceptions;
using AppCore.HelperEntities;
using AppCore.Interfaces;
using AutoMapper;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
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

        public async Task<(PagedList<User>, IEnumerable<UserForListDto>)> GetUsers(int id, UserParams userParams)
        {
            var userFromRepo = await _userRepo.GetUser(id);

            userParams.UserId = id;

            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = userFromRepo.Gender == "male" ? "female" : "male";
            }

            var users = await _userRepo.GetUsers(userParams);

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            return (users, usersToReturn);
        }

        public async Task UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            var userFromRepo = await _userRepo.GetUser(id);

            _mapper.Map(userForUpdateDto, userFromRepo);

            try
            {
                await _userRepo.SaveAll();
            }
            catch
            {
                throw new SaveDataException();
            }
        }

        public async Task<UserForDetailedDTO> GetUser(int id)
        {
            var user = await _userRepo.GetUser(id);

            var userToReturn = _mapper.Map<UserForDetailedDTO>(user);

            return userToReturn;
        }

        public async Task LikeUser(int id, int recipientId)
        {
            var like = await _userRepo.GetLike(id, recipientId);

            if (like != null)
            {
                throw new AlreadyExistsException();
            }

            if (await _userRepo.GetUser(recipientId) == null)
            {
                throw new NotFoundException();
            }

            like = new Like
            {
                LikerId = id,
                LikeeId = recipientId
            };

            _likeRepo.Add(like);

            try
            {
                await _userRepo.SaveAll();
                    }
            catch 
            {
                throw new SaveDataException();
            }
        }

        public async Task LogActivity(int id)
        {
            var user = await _userRepo.GetUser(id);

            user.LastActive = DateTime.Now;

            await _userRepo.SaveAll();
        }

    }
}
