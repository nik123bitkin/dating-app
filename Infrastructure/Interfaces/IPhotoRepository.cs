using AppCore.Entities;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IPhotoRepository : IDataRepository<Photo>
    {
        Task<Photo> GetMainPhotoForUser(int userId);
    }
}
