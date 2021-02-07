using AppCore.Entities;
using System.Threading.Tasks;

namespace AppCore.Interfaces
{
    public interface IPhotoRepository : IDataRepository
    {
        Task<Photo> GetPhoto(int id);
        Task<Photo> GetMainPhotoForUser(int userId);
    }
}
