using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IDataRepository<T> where T: class
    {
        void Add(T entity);
        void Delete(T entity);
        Task<T> GetById(object id);
        Task<bool> SaveAll();
    }
}
