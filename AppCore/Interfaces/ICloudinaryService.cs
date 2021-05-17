using AppCore.DTOs;
using AppCore.Entities;

namespace AppCore.Interfaces
{
    public interface ICloudinaryService
    {
        void UploadImage(PhotoForCreationDto photoForCreationDto);

        void DeleteImage(Photo photoForDelete);
    }
}
