using System.Threading.Tasks;
using AppCore.Entities;
using AppCore.HelperEntities;

namespace AppCore.Interfaces
{
    public interface IUserRepository : IDataRepository<User>
    {
        Task<PagedList<User>> GetUsersAsync(UserParams userParams);

        Task<User> GetUserAsync(int id);

        Task<Like> GetLikeAsync(int userId, int recipientId);
    }
}
