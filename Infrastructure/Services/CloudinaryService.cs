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

        public void DeleteImage(Photo photoForDelete)
        {
            var deleteParams = new DeletionParams(photoForDelete.PublicId);

            var result = _cloudinary.Destroy(deleteParams);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new System.Exception("Cloudinary service failed to delete the image");
            }
        }

        public void UploadImage(PhotoForCreationDto photoForCreationDto)
        {
            var file = photoForCreationDto.File;

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.Name, stream),
                    Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                };

                var uploadResult = _cloudinary.Upload(uploadParams);

                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    photoForCreationDto.Url = uploadResult.Url.OriginalString;
                    photoForCreationDto.PublicId = uploadResult.PublicId;
                }
                else
                {
                    throw new System.Exception("Cloudinary service failed to upload the image");
                }
            }
        }
    }
}
