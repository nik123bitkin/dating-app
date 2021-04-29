using System.Linq;
using System.Threading.Tasks;
using AppCore.DTOs;
using AppCore.Entities;
using AppCore.Exceptions;
using AppCore.Interfaces;
using AutoMapper;

namespace AppCore.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IPhotoRepository _photoRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;

        public PhotoService(IPhotoRepository photoRepo, IUserRepository userRepo, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _photoRepo = photoRepo;
            _userRepo = userRepo;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<PhotoForReturnDto> AddForUser(int userId, PhotoForCreationDto photoForCreationDto)
        {
            var userFromRepo = await _userRepo.GetUser(userId);

            var uploadResult = _cloudinaryService.UploadImage(photoForCreationDto);

            photoForCreationDto.Url = uploadResult.Url.OriginalString;
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDto);
            if (!userFromRepo.Photos.Any(u => u.IsMain))
            {
                photo.IsMain = true;
            }

            userFromRepo.Photos.Add(photo);
            try
            {
                await _photoRepo.SaveAll();
                var photoForReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return photoForReturn;
            }
            catch
            {
                throw;
            }
        }

        public async Task DeleteForUser(int userId, int id)
        {
            var userFromRepo = await _userRepo.GetUser(userId);

            if (!userFromRepo.Photos.Any(p => p.Id == id))
            {
                throw new NotFoundException();
            }

            var photoFromRepo = await _photoRepo.GetById(id);
            if (photoFromRepo.IsMain)
            {
                throw new ForbiddenActionException();
            }

            if (photoFromRepo.PublicId != null)
            {
                var result = _cloudinaryService.DeleteImage(photoFromRepo);

                if (result.Result == "ok")
                {
                    _photoRepo.Delete(photoFromRepo);
                }
            }

            if (photoFromRepo.PublicId == null)
            {
                _photoRepo.Delete(photoFromRepo);
            }

            try
            {
                await _photoRepo.SaveAll();
            }
            catch
            {
                throw;
            }
        }

        public async Task<PhotoForReturnDto> GetPhoto(int id)
        {
            var photoFromRepo = await _photoRepo.GetById(id);

            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return photo;
        }

        public async Task SetAsMain(int userId, int id)
        {
            var userFromRepo = await _userRepo.GetUser(userId);

            if (!userFromRepo.Photos.Any(p => p.Id == id))
            {
                throw new NotFoundException();
            }

            var photoFromRepo = await _photoRepo.GetById(id);
            if (photoFromRepo.IsMain)
            {
                throw new AlreadyExistsException();
            }

            var currentMainPhoto = await _photoRepo.GetMainPhotoForUser(userId);
            currentMainPhoto.IsMain = false;
            photoFromRepo.IsMain = true;

            try
            {
                await _photoRepo.SaveAll();
            }
            catch
            {
                throw;
            }
        }
    }
}
