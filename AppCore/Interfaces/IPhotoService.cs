using System.Threading.Tasks;
using AppCore.DTOs;

namespace AppCore.Interfaces
{
    public interface IPhotoService
    {
        Task<PhotoForReturnDto> GetPhotoAsync(int id);

        Task<PhotoForReturnDto> AddForUserAsync(int userId, PhotoForCreationDto photoForCreationDto);

        Task DeleteForUserAsync(int userId, int id);

        Task SetAsMainAsync(int userId, int id);
    }
}
