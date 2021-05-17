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

        public async Task<PhotoForReturnDto> AddForUserAsync(int userId, PhotoForCreationDto photoForCreationDto)
        {
            _cloudinaryService.UploadImage(photoForCreationDto);

            var photo = _mapper.Map<Photo>(photoForCreationDto);
            photo.UserId = userId;

            var mainPhoto = _photoRepo.GetMainPhotoForUserAsync(userId);
            if (mainPhoto == null)
            {
                photo.IsMain = true;
            }

            _photoRepo.Add(photo);
            await _photoRepo.SaveAllAsync();

            var photoForReturn = _mapper.Map<PhotoForReturnDto>(photo);
            return photoForReturn;
        }

        public async Task DeleteForUserAsync(int userId, int id)
        {
            var userFromRepo = await _userRepo.GetByIdAsync(userId);

            if (!userFromRepo.Photos.Any(p => p.Id == id))
            {
                throw new NotFoundException("Failed to delete photo. Photo with such ID not found.");
            }

            var photoFromRepo = await _photoRepo.GetByIdAsync(id);
            if (photoFromRepo.IsMain)
            {
                throw new ForbiddenActionException("Unable to delete main photo.");
            }

            if (photoFromRepo.PublicId != null)
            {
                _cloudinaryService.DeleteImage(photoFromRepo);
            }

            _photoRepo.Delete(photoFromRepo);

            await _photoRepo.SaveAllAsync();
        }

        public async Task<PhotoForReturnDto> GetPhotoAsync(int id)
        {
            var photoFromRepo = await _photoRepo.GetByIdAsync(id);

            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return photo;
        }

        public async Task SetAsMainAsync(int userId, int id)
        {
            var userFromRepo = await _userRepo.GetUserAsync(userId);

            if (!userFromRepo.Photos.Any(p => p.Id == id))
            {
                throw new NotFoundException("Failed to set main photo. Photo with such ID not found.");
            }

            var photoFromRepo = await _photoRepo.GetByIdAsync(id);
            if (photoFromRepo.IsMain)
            {
                throw new AlreadyExistsException("Failed to set main photo. This photo is main already.");
            }

            var currentMainPhoto = await _photoRepo.GetMainPhotoForUserAsync(userId);
            currentMainPhoto.IsMain = false;
            photoFromRepo.IsMain = true;

            await _photoRepo.SaveAllAsync();
        }
    }
}
