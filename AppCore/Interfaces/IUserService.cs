using System.Collections.Generic;
using System.Threading.Tasks;
using AppCore.DTOs;
using AppCore.Entities;
using AppCore.HelperEntities;

namespace AppCore.Interfaces
{
    public interface IUserService
    {
        Task<(PagedList<User>, IEnumerable<UserForListDto>)> GetUsersAsync(int id, UserParams userParams);

        Task UpdateUserAsync(int id, UserForUpdateDto userForUpdateDto);

        Task LikeUserAsync(int id, int recipientId);

        Task<UserForDetailedDto> GetUserAsync(int id);

        Task LogActivityAsync(int id);
    }
}
