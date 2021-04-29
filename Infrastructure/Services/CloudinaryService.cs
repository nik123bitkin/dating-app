using AppCore.DTOs;
using AppCore.Entities;
using AppCore.HelperEntities;
using AppCore.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly IOptions<CloudinarySettings> _cloudinarySettings;
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> cloudinarySettings)
        {
            _cloudinarySettings = cloudinarySettings;

            Account acc = new Account(
                _cloudinarySettings.Value.CloudName,
                _cloudinarySettings.Value.ApiKey,
                _cloudinarySettings.Value.ApiSecret);

            _cloudinary = new Cloudinary(acc);
        }

        public DeletionResult DeleteImage(Photo photoForDelete)
        {
            var deleteParams = new DeletionParams(photoForDelete.PublicId);

            return _cloudinary.Destroy(deleteParams);
        }

        public ImageUploadResult UploadImage(PhotoForCreationDto photoForCreationDto)
        {
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

            return uploadResult;
        }
    }
}
