using AppCore.Interfaces;
using Infrastructure.Context;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class DataRepository : IDataRepository
    {
        protected readonly DataContext _context;
        public DataRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
