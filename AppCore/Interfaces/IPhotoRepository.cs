using System.Threading.Tasks;
using AppCore.Entities;

namespace AppCore.Interfaces
{
    public interface IPhotoRepository : IDataRepository<Photo>
    {
        Task<Photo> GetMainPhotoForUser(int userId);
    }
}
