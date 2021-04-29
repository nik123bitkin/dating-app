using AppCore.DTOs;
using AppCore.Entities;
using CloudinaryDotNet.Actions;

namespace AppCore.Interfaces
{
    public interface ICloudinaryService
    {
        ImageUploadResult UploadImage(PhotoForCreationDto photoForCreationDto);

        DeletionResult DeleteImage(Photo photoForDelete);
    }
}
