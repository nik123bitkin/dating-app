using AppCore.Entities;
using AppCore.Interfaces;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class LikeRepository : DataRepository<Like>, ILikeRepository
    {
        public LikeRepository(DataContext context)
            : base(context)
        {
        }
    }
}
