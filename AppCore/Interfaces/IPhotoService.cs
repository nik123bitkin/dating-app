using AppCore.DTOs;
using AppCore.Entities;
using AppCore.HelperEntities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppCore.Interfaces
{
    public interface IPhotoService
    {
        Task<PhotoForReturnDto> GetPhoto(int id);
        Task<PhotoForReturnDto> AddForUser(int userId, PhotoForCreationDto photoForCreationDto);
        Task DeleteForUser(int userId, int id);
        Task SetAsMain(int userId, int id);
    }
}
