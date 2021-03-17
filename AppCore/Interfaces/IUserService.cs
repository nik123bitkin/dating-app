using AppCore.DTOs;
using AppCore.Entities;
using AppCore.HelperEntities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppCore.Interfaces
{
    public interface IUserService
    {
        Task<(PagedList<User>, IEnumerable<UserForListDto>)> GetUsers(int id, UserParams userParams);
        Task UpdateUser(int id, UserForUpdateDto userForUpdateDto);
        Task LikeUser(int id, int recipientId);
        Task<UserForDetailedDTO> GetUser(int id);
        Task LogActivity(int id);
    }
}
