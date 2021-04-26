using AppCore.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class LikeRepository : DataRepository<Like>, ILikeRepository
    {
        public LikeRepository(DataContext context) : base(context)
        { }
    }
}
