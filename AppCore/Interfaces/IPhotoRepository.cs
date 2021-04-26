using AppCore.Entities;
using System.Threading.Tasks;

namespace AppCore.Interfaces
{
    public interface IPhotoRepository : IDataRepository<Photo>
    {
        Task<Photo> GetMainPhotoForUser(int userId);
    }
}
