using System.Threading.Tasks;
using AppCore.Entities;
using AppCore.HelperEntities;

namespace AppCore.Interfaces
{
    public interface IUserRepository : IDataRepository<User>
    {
        Task<PagedList<User>> GetUsers(UserParams userParams);

        Task<User> GetUser(int id);

        Task<Like> GetLike(int userId, int recipientId);
    }
}
