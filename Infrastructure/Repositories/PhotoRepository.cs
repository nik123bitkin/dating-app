using AppCore.Entities;
using AppCore.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PhotoRepository : DataRepository<Photo>, IPhotoRepository
    {
        public PhotoRepository(DataContext context) : base(context)
        { }
        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            return await _context.Photos.Where(u => u.UserId == userId)
                .FirstOrDefaultAsync(p => p.IsMain);
        }
    }
}
