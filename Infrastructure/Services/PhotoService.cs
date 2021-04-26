using AppCore.DTOs;
using AppCore.Entities;
using AppCore.Exceptions;
using AppCore.HelperEntities;
using AppCore.Interfaces;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IPhotoRepository _photoRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        private readonly IOptions<CloudinarySettings> _cloudinarySettings;
        private Cloudinary _cloudinary;

        public PhotoService(IPhotoRepository photoRepo, IUserRepository userRepo, IMapper mapper,
            IOptions<CloudinarySettings> cloudinarySettings)
        {
            _photoRepo = photoRepo;
            _userRepo = userRepo;
            _mapper = mapper;
            _cloudinarySettings = cloudinarySettings;

            Account acc = new Account(
                _cloudinarySettings.Value.CloudName,
                _cloudinarySettings.Value.ApiKey,
                _cloudinarySettings.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        public async Task<PhotoForReturnDto> AddForUser(int userId, PhotoForCreationDto photoForCreationDto)
        {
            var userFromRepo = await _userRepo.GetUser(userId);

            var file = photoForCreationDto.File;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

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
                //return CreatedAtRoute("GetPhoto", new { userId = userId, id = photo.Id }, photoForReturn);
                
            }
            catch
            {
                throw new SaveDataException();
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
                var deleteParams = new DeletionParams(photoFromRepo.PublicId);

                var result = _cloudinary.Destroy(deleteParams);

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
                throw new SaveDataException();
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
                throw new SaveDataException();
            }
        }
    }
}
